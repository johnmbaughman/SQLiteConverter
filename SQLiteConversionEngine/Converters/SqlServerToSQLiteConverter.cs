using SQLiteConversionEngine.ConversionData;
using SQLiteConversionEngine.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;

namespace SQLiteConversionEngine.Converters
{
	public sealed class SqlServerToSQLiteConverter : BaseConverter
	{
		#region Public Methods
		public SqlServerToSQLiteConverter()
		{
			//Logging.Log(LogLevel.Info, "SqlServerConverter initialized");
		}

		public SqlServerToSQLiteConverter(List<TableToLoad> tablesToLoad)
		{
			//Logging.Log(LogLevel.Info, "SqlServerConverter initializing");
			base.TablesToLoad = tablesToLoad;
			//Logging.Log(LogLevel.Debug, string.Format("Tables to load count: {0}", TablesToLoad.Count));
			//Logging.Log(LogLevel.Info, "SqlServerConverter initialized");
		}

		public override void ConvertToDatabase(string sqlServerConnectionString, string sqliteConnectionString, SqlConversionHandler sqlConversionHandler, SqlTableSelectionHandler sqlTableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers)
		{
			using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
			{
				ConvertToDatabase(sqlServerConnectionString, sqliteConnection, sqlConversionHandler, sqlTableSelectionHandler, failedViewDefinitionHandler, createTriggers);
			}
		}

		public override void ConvertToDatabase(string sqlServerConnectionString, SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler, SqlTableSelectionHandler sqlTableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers)
		{
			using (SqlConnection sqlServerConnection = new SqlConnection(sqlServerConnectionString))
			{
				ConvertToDatabase(sqlServerConnection, sqliteConnection, sqlConversionHandler, sqlTableSelectionHandler, failedViewDefinitionHandler, createTriggers);
			}
		}

		public override void ConvertToDatabase(SqlConnection sqlServerConnection, SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler, SqlTableSelectionHandler sqlTableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers)
		{
			// Clear cancelled flag
			_cancelled = false;

			//WaitCallback wc = new WaitCallback(delegate(object state)
			//{
			try
			{
				_isActive = true;
				ConvertSourceDatabaseToDestination(sqlServerConnection, sqliteConnection, sqlConversionHandler, sqlTableSelectionHandler, failedViewDefinitionHandler, createTriggers);
				_isActive = false;
				sqlConversionHandler(true, true, 100, "Finished converting database");
			}
			catch (Exception ex)
			{
				//Logging.Log(LogLevel.Error, string.Format("Failed to convert SQL Server database to SQLite database: {0}", FileLogger.GetInnerException(ex).Message));
				//Logging.HandleException(ex);
				_isActive = false;
				sqlConversionHandler(true, false, 100, ex.Message);
			}
			//});
			//ThreadPool.QueueUserWorkItem(wc);
		}
		#endregion

		#region Private Methods
		protected override void ConvertSourceDatabaseToDestination(SqlConnection sqlServerConnection,
			System.Data.SQLite.SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler,
			SqlTableSelectionHandler sqlTableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler,
			bool createTriggers)
		{
			using (sqlServerConnection)
			{
				// Read the schema of the SQL Server database into a memory structure
				DatabaseSchema ds = ReadSourceSchema(sqlServerConnection, sqlConversionHandler, sqlTableSelectionHandler);

				// Create the SQLite database and apply the schema
				CreateSQLiteDatabase(sqliteConnection, ds, sqlConversionHandler, failedViewDefinitionHandler);

				// Copy all rows from SQL Server tables to the newly created SQLite database
				CopySourceDatabaseRowsToDestination(sqlServerConnection, sqliteConnection, ds.Tables, sqlConversionHandler);

				// Add triggers based on foreign key constraints
				if (createTriggers)
					AddTriggersForForeignKeys(sqliteConnection, ds.Tables, sqlConversionHandler);
			}
		}
			
		protected override void CopySourceDatabaseRowsToDestination(
			SqlConnection sqlServerConnection, SQLiteConnection sqliteConnection, List<TableSchema> schema,
			SqlConversionHandler handler)
		{
			CheckCancelled();
			handler(false, true, 0, "Preparing to insert tables...");
			//Logging.Log(LogLevel.Debug, "preparing to insert tables ...");

			// Connect to the SQL Server database
			sqlServerConnection.Open();

			// Go over all tables in the schema and copy their rows
			for (int i = 0; i < schema.Count; i++)
			{
				if (schema[i].DataAction == DataActionEnum.Ignore) continue;
				SQLiteTransaction tx = sqliteConnection.BeginTransaction();
				try
				{
					string tableQuery = BuildSourceTableQuery(schema[i]);
					SqlCommand query = new SqlCommand(tableQuery, sqlServerConnection);
					using (SqlDataReader reader = query.ExecuteReader())
					{
						SQLiteCommand insert = BuildSQLiteInsert(schema[i]);
						int counter = 0;
						while (reader.Read())
						{
							insert.Connection = sqliteConnection;
							insert.Transaction = tx;
							List<string> pnames = new List<string>();
							for (int j = 0; j < schema[i].Columns.Count; j++)
							{
								string pname = "@" + GetNormalizedName(schema[i].Columns[j].ColumnName, pnames);
								insert.Parameters[pname].Value = CastValueForColumn(reader[j], schema[i].Columns[j]);
								pnames.Add(pname);
							}
							insert.ExecuteNonQuery();
							counter++;
							if (counter % 1000 == 0)
							{
								CheckCancelled();
								tx.Commit();
								handler(false, true, (int)(100.0 * i / schema.Count),
									"Added " + counter + " rows to table " + schema[i].TableName + " so far");
								tx = sqliteConnection.BeginTransaction();
							}
						}
					}

					CheckCancelled();
					tx.Commit();

					handler(false, true, (int)(100.0 * i / schema.Count), "Finished inserting rows for table " + schema[i].TableName);
					//Logging.Log(LogLevel.Debug, "finished inserting all rows for table [" + schema[i].TableName + "]");
				}
				catch (Exception ex)
				{
					//Logging.Log(LogLevel.Error, string.Format("unexpected exception: {0}", FileLogger.GetInnerException(ex).Message));
					//Logging.HandleException(ex);
					tx.Rollback();
					throw;
				}
			}

			sqlServerConnection.Close();
		}

		protected override string BuildSourceTableQuery(TableSchema ts)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("SELECT ");
			for (int i = 0; i < ts.Columns.Count; i++)
			{
				sb.Append("[" + ts.Columns[i].ColumnName + "]");
				if (i < ts.Columns.Count - 1)
					sb.Append(", ");
			}
			sb.Append(" FROM " + ts.TableSchemaName + "." + "[" + ts.TableName + "]");
			return sb.ToString();
		}
		
		protected override DatabaseSchema ReadSourceSchema(SqlConnection sqlServerConnection, SqlConversionHandler handler,
			SqlTableSelectionHandler selectionHandler)
		{
			// First step is to read the names of all tables in the database
			List<TableSchema> tables = new List<TableSchema>();

			sqlServerConnection.Open();

			//Logging.Log(LogLevel.Debug, "SQL Server Connection initialized");

			List<string> tableNames = new List<string>();
			List<string> tblschema = new List<string>();

			// Initialize in clause for schemas and tables.
			//Logging.Log(LogLevel.Debug, "Intializing schemas and tables to load where clauses");
			string schemasToLoad = Utilities.ContainsSchemaInfo(TablesToLoad) ? Utilities.ConvertTableToLoadToInClause(TablesToLoad, true, "TABLE_SCHEMA").ToUpper() : string.Empty;
			//if (schemasToLoad != string.Empty) //Logging.Log(LogLevel.Debug, string.Format("Schemas IN clause: {0}", schemasToLoad));
			string tablesToLoad = TablesToLoad.Count > 0 ? Utilities.ConvertTableToLoadToInClause(TablesToLoad, false, "TABLE_NAME").ToUpper() : string.Empty;
			//if (tablesToLoad != string.Empty) //Logging.Log(LogLevel.Debug, string.Format("Tables IN clause: {0}", tablesToLoad));

			string whereClause = string.Empty;
			if (schemasToLoad != string.Empty && tablesToLoad != string.Empty)
				whereClause = string.Format(" AND {0} AND {1} ", schemasToLoad, tablesToLoad);
			else if (schemasToLoad != string.Empty && tablesToLoad == string.Empty)
				whereClause = string.Format(" AND {0} ", schemasToLoad);
			else if (schemasToLoad == string.Empty && tablesToLoad != string.Empty)
				whereClause = string.Format(" AND {0} ", tablesToLoad);

			//Logging.Log(LogLevel.Debug, "Intializing SQL statement");
			// This command will read the names of all tables in the database
			string sqlQuery = string.Format(@"select * from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE = 'BASE TABLE'{0}", whereClause);
			//Logging.Log(LogLevel.Debug, string.Format("Sql Server INFORMATION_SCHEMA.TABLES query: \n\n{0}\n\n", sqlQuery));

			SqlCommand cmd = new SqlCommand(sqlQuery, sqlServerConnection);
			//Logging.Log(LogLevel.Debug, "SqlCommand initialized");
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					tableNames.Add((string)reader["TABLE_NAME"]);
					tblschema.Add((string)reader["TABLE_SCHEMA"]);
				}
			}

			// Next step is to use ADO APIs to query the schema of each table.
			int count = 0;
			for (int i = 0; i < tableNames.Count; i++)
			{
				string tname = tableNames[i];
				string tschma = tblschema[i];
				string fullName = tschma + "." + tname;
				// Load only the tables in TablesToLoad.
				if (TablesToLoad.Count > 0 && !TablesToLoad.Exists(t => t.SqlServerFullName.ToLower() == fullName.ToLower())) continue;

				TableSchema ts = CreateTableSchema(sqlServerConnection, tname, tschma);
				CreateForeignKeySchema(sqlServerConnection, ts);
				tables.Add(ts);
				count++;
				CheckCancelled();
				handler(false, true, (int)(count * 50.0 / tableNames.Count), "Parsed table " + tname);

				//Logging.Log(LogLevel.Debug, "parsed table schema for [" + tname + "]");
			}
			sqlServerConnection.Close();

			//Logging.Log(LogLevel.Debug, "finished parsing all tables in SQL Server schema");

			// Allow the user a chance to select which tables to convert, only if the TablesToLoad list isn't defined.
			if (selectionHandler != null && TablesToLoad.Count > 0)
			{
				List<TableSchema> updated = selectionHandler(tables);
				if (updated != null)
					tables = updated;
			}

			Regex removedbo = new Regex(@"dbo\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			// Continue and read all of the views in the database
			List<ViewSchema> views = new List<ViewSchema>();

			sqlServerConnection.Open();
			cmd = new SqlCommand(@"SELECT TABLE_NAME, VIEW_DEFINITION  from INFORMATION_SCHEMA.VIEWS", sqlServerConnection);
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				count = 0;
				while (reader.Read())
				{
					ViewSchema vs = new ViewSchema();
					vs.ViewName = (string)reader["TABLE_NAME"];
					vs.ViewSQL = (string)reader["VIEW_DEFINITION"];

					// Remove all ".dbo" strings from the view definition
					vs.ViewSQL = removedbo.Replace(vs.ViewSQL, string.Empty);

					views.Add(vs);

					count++;
					CheckCancelled();
					handler(false, true, 50 + (int)(count * 50.0 / views.Count), "Parsed view " + vs.ViewName);

					//Logging.Log(LogLevel.Debug, "parsed view schema for [" + vs.ViewName + "]");
				}
			}
			sqlServerConnection.Close();

			DatabaseSchema ds = new DatabaseSchema();
			ds.Tables = tables;
			ds.Views = views;
			return ds;
		}

		protected override TableSchema CreateTableSchema(IDbConnection conn, string tableName, string tschma)
		{
			TableSchema res = new TableSchema();
			res.TableName = tableName;
			res.TableSchemaName = tschma;
			res.DataAction = TablesToLoad.Find((TableToLoad t) => t.SqlServerFullName.ToLower().Equals((tschma + "." + tableName).ToLower())).SQLiteDataAction;
			res.Columns = new List<ColumnSchema>();
			SqlCommand cmd = new SqlCommand(@"SELECT COLUMN_NAME,COLUMN_DEFAULT,IS_NULLABLE,DATA_TYPE, " +
				@" (columnproperty(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity')) AS [IDENT], " +
				@"CHARACTER_MAXIMUM_LENGTH AS CSIZE " +
				"FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + tschma + "' AND TABLE_NAME = '" + tableName + "' ORDER BY " +
				"ORDINAL_POSITION ASC", (SqlConnection)conn);
			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					object tmp = reader["COLUMN_NAME"];
					if (tmp is DBNull)
						continue;
					string colName = (string)reader["COLUMN_NAME"];

					tmp = reader["COLUMN_DEFAULT"];
					string colDefault;
					if (tmp is DBNull)
						colDefault = string.Empty;
					else
						colDefault = (string)tmp;

					tmp = reader["IS_NULLABLE"];
					bool isNullable = ((string)tmp == "YES");
					string dataType = (string)reader["DATA_TYPE"];
					bool isIdentity = false;
					if (reader["IDENT"] != DBNull.Value)
						isIdentity = ((int)reader["IDENT"]) == 1 ? true : false;
					int length = reader["CSIZE"] != DBNull.Value ? Convert.ToInt32(reader["CSIZE"]) : 0;

					ValidateDataType(dataType);

					// Note that not all data type names need to be converted because
					// SQLite establishes type affinity by searching certain strings
					// in the type name. For example - everything containing the string
					// 'int' in its type name will be assigned an INTEGER affinity
					if (dataType == "timestamp")
						dataType = "blob";
					else if (dataType == "datetime" || dataType == "smalldatetime")
						dataType = "datetime";
					else if (dataType == "decimal")
						dataType = "numeric";
					else if (dataType == "money" || dataType == "smallmoney")
						dataType = "numeric";
					else if (dataType == "binary" || dataType == "varbinary" ||
						dataType == "image")
						dataType = "blob";
					else if (dataType == "tinyint")
						dataType = "smallint";
					else if (dataType == "bigint")
						dataType = "integer";
					else if (dataType == "sql_variant")
						dataType = "blob";
					else if (dataType == "xml")
						dataType = "varchar";
					else if (dataType == "uniqueidentifier")
						dataType = "guid";
					else if (dataType == "ntext")
						dataType = "text";
					else if (dataType == "nchar")
						dataType = "char";
					else if (dataType == "datetime2")
						dataType = "datetime2";
					else if (dataType == "date")
						dataType = "date";
					else if (dataType == "time")
						dataType = "time";

					if (dataType == "bit" || dataType == "int")
					{
						if (colDefault == "('False')")
							colDefault = "(0)";
						else if (colDefault == "('True')")
							colDefault = "(1)";
					}

					colDefault = FixDefaultValueString(colDefault);

					ColumnSchema col = new ColumnSchema();
					col.ColumnName = colName;
					col.ColumnType = dataType;
					col.Length = length;
					col.IsNullable = isNullable;
					col.IsIdentity = isIdentity;
					col.DefaultValue = AdjustDefaultValue(colDefault);
					res.Columns.Add(col);
				}
			}

			// Find PRIMARY KEY information
			SqlCommand cmd2 = new SqlCommand(@"EXEC sp_pkeys '" + tableName + "'", (SqlConnection)conn);
			using (SqlDataReader reader = cmd2.ExecuteReader())
			{
				res.PrimaryKey = new List<string>();
				while (reader.Read())
				{
					string colName = (string)reader["COLUMN_NAME"];
					res.PrimaryKey.Add(colName);
				}
			}

			// Find COLLATE information for all columns in the table
			SqlCommand cmd4 = new SqlCommand(
				@"EXEC sp_tablecollations '" + tschma + "." + tableName + "'", (SqlConnection)conn);
			using (SqlDataReader reader = cmd4.ExecuteReader())
			{
				while (reader.Read())
				{
					bool? isCaseSensitive = null;
					string colName = (string)reader["name"];
					if (reader["tds_collation"] != DBNull.Value)
					{
						byte[] mask = (byte[])reader["tds_collation"];
						if ((mask[2] & 0x10) != 0)
							isCaseSensitive = false;
						else
							isCaseSensitive = true;
					}

					if (isCaseSensitive.HasValue)
					{
						// Update the corresponding column schema.
						foreach (ColumnSchema csc in res.Columns)
						{
							if (csc.ColumnName == colName)
							{
								csc.IsCaseSensitivite = isCaseSensitive;
								break;
							}
						}
					}
				}
			}

			try
			{
				// Find index information
				SqlCommand cmd3 = new SqlCommand(
					@"exec sp_helpindex '" + tschma + "." + tableName + "'", (SqlConnection)conn);
				using (SqlDataReader reader = cmd3.ExecuteReader())
				{
					res.Indexes = new List<IndexSchema>();
					while (reader.Read())
					{
						string indexName = (string)reader["index_name"];
						string desc = (string)reader["index_description"];
						string keys = (string)reader["index_keys"];

						// Don't add the index if it is actually a primary key index
						if (desc.Contains("primary key"))
							continue;

						IndexSchema index = BuildIndexSchema(indexName, desc, keys);
						res.Indexes.Add(index);
					}
				}
			}
			catch (Exception ex)
			{
				//Logging.Log(LogLevel.Warn, "failed to read index information for table [" + tableName + "]");
			}

			return res;
		}

		protected override void CreateForeignKeySchema(IDbConnection conn, TableSchema ts)
		{
			ts.ForeignKeys = new List<ForeignKeySchema>();

			SqlCommand cmd = new SqlCommand(
				@"SELECT " +
				@"  ColumnName = CU.COLUMN_NAME, " +
				@"  ForeignTableName  = PK.TABLE_NAME, " +
				@"  ForeignColumnName = PT.COLUMN_NAME, " +
				@"  DeleteRule = C.DELETE_RULE, " +
				@"  IsNullable = COL.IS_NULLABLE " +
				@"FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C " +
				@"INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
				@"INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME " +
				@"INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME " +
				@"INNER JOIN " +
				@"  ( " +
				@"    SELECT i1.TABLE_NAME, i2.COLUMN_NAME " +
				@"    FROM  INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 " +
				@"    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME " +
				@"    WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' " +
				@"  ) " +
				@"PT ON PT.TABLE_NAME = PK.TABLE_NAME " +
				@"INNER JOIN INFORMATION_SCHEMA.COLUMNS AS COL ON CU.COLUMN_NAME = COL.COLUMN_NAME AND FK.TABLE_NAME = COL.TABLE_NAME " +
				@"WHERE FK.Table_NAME='" + ts.TableName + "'", (SqlConnection)conn);

			using (SqlDataReader reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					ForeignKeySchema fkc = new ForeignKeySchema();
					fkc.ColumnName = (string)reader["ColumnName"];
					fkc.ForeignTableName = (string)reader["ForeignTableName"];
					fkc.ForeignColumnName = (string)reader["ForeignColumnName"];
					fkc.CascadeOnDelete = (string)reader["DeleteRule"] == "CASCADE";
					fkc.IsNullable = (string)reader["IsNullable"] == "YES";
					fkc.TableName = ts.TableName;
					ts.ForeignKeys.Add(fkc);
				}
			}
		}
		#endregion
	}
}
