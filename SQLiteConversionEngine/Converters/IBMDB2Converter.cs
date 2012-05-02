using ConvertToSQLiteEngine.ConversionData;
using ConvertToSQLiteEngine.Utility;
using LoggingEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using IBM.Data.DB2.iSeries;

namespace ConvertToSQLiteEngine.Converters
{
	public sealed class IBMDB2Converter : BaseConverter
	{
		#region Public methods
		public IBMDB2Converter()
		{
			Logging.Log(LogLevel.Info, "IBMDB2Converter initialized");
		}

		public IBMDB2Converter(List<string> tablesToLoad)
		{
			Logging.Log(LogLevel.Info, "IBMDB2Converter initializing");
			TablesToLoad = tablesToLoad;
			Logging.Log(LogLevel.Debug, string.Format("Schemas to load count: {0}", SchemasToLoad.Count));
			Utilities.PrintListToLog(SchemasToLoad);
			Logging.Log(LogLevel.Debug, string.Format("Tables to load count: {0}", TablesToLoad.Count));
			Utilities.PrintListToLog(TablesToLoad);
			Logging.Log(LogLevel.Info, "IBMDB2Converter initialized");
		}

		public override void ConvertToSQLiteDatabase(string db2ConnectionString, string sqliteDbPath, string sqliteDbPassword, SqlConversionHandler sqlConversionHandler, SqlTableSelectionHandler sqlTableSelecttionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers)
		{
			// Clear cancelled flag
			_cancelled = false;

			WaitCallback wc = new WaitCallback(delegate(object state)
			{
				try
				{
					_isActive = true;
					ConvertSourceDatabaseToSQLiteFile(db2ConnectionString, sqliteDbPath, sqliteDbPassword, sqlConversionHandler, sqlTableSelecttionHandler, failedViewDefinitionHandler, createTriggers);
					_isActive = false;
					sqlConversionHandler(true, true, 100, "Finished converting database");
				}
				catch (Exception ex)
				{
					Logging.Log(LogLevel.Error, string.Format("Failed to convert DB2 database to SQLite database: {0}", FileLogger.GetInnerException(ex).Message));
					_isActive = false;
					sqlConversionHandler(true, false, 100, ex.Message);
				} // catch
			});
			ThreadPool.QueueUserWorkItem(wc);
		}

		public override void ConvertToSQLiteDatabase(string db2ConnectionString, System.Data.SQLite.SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler, SqlTableSelectionHandler sqlTableSelecttionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers)
		{
			// Clear cancelled flag
			_cancelled = false;

			//WaitCallback wc = new WaitCallback(delegate(object state)
			//{
			try
			{
				_isActive = true;
				ConvertSourceDatabaseToSQLiteFile(db2ConnectionString, sqliteConnection, sqlConversionHandler, sqlTableSelecttionHandler, failedViewDefinitionHandler, createTriggers);
				_isActive = false;
				sqlConversionHandler(true, true, 100, "Finished converting database");
			}
			catch (Exception ex)
			{
				Logging.Log(LogLevel.Error, string.Format("Failed to convert DB2 database to SQLite database: {0}", FileLogger.GetInnerException(ex).Message));
				_isActive = false;
				sqlConversionHandler(true, false, 100, ex.Message);
			} // catch
			//});
			//ThreadPool.QueueUserWorkItem(wc);
		} 
		#endregion

		#region Protected methods
		protected override string BuildSourceTableQuery(ConversionData.TableSchema tableSchema)
		{
			throw new NotImplementedException();
		}

		protected override void ConvertSourceDatabaseToSQLiteFile(string db2ConnectionString, 
			string sqliteDbPath, string sqliteDbPassword, SqlConversionHandler sqlConversionHandler, 
			SqlTableSelectionHandler sqlTableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, 
			bool createTriggers)
		{
			// Delete the target file if it exists already.
			if (File.Exists(sqliteDbPath))
				File.Delete(sqliteDbPath);

			// Read the schema of the SQL Server database into a memory structure
			DatabaseSchema ds = ReadSourceSchema(db2ConnectionString, sqlConversionHandler, sqlTableSelectionHandler);

			// Create the SQLite database and apply the schema
			CreateSQLiteDatabase(sqliteDbPath, ds, sqliteDbPassword, sqlConversionHandler, failedViewDefinitionHandler);

			// Copy all rows from SQL Server tables to the newly created SQLite database
			CopySourceDatabaseRowsToSQLiteDatabase(db2ConnectionString, sqliteDbPath, ds.Tables, sqliteDbPassword, sqlConversionHandler);

			// Add triggers based on foreign key constraints
			if (createTriggers)
				AddTriggersForForeignKeys(sqliteDbPath, ds.Tables, sqliteDbPassword, sqlConversionHandler);
		}

		protected override void ConvertSourceDatabaseToSQLiteFile(string db2ConnectionString, 
			System.Data.SQLite.SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler, 
			SqlTableSelectionHandler sqlTableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, 
			bool createTriggers)
		{
			// Read the schema of the SQL Server database into a memory structure
			DatabaseSchema ds = ReadSourceSchema(db2ConnectionString, sqlConversionHandler, sqlTableSelectionHandler);

			// Create the SQLite database and apply the schema
			CreateSQLiteDatabase(sqliteConnection, ds, sqlConversionHandler, failedViewDefinitionHandler);

			// Copy all rows from SQL Server tables to the newly created SQLite database
			CopySourceDatabaseRowsToSQLiteDatabase(db2ConnectionString, sqliteConnection, ds.Tables, sqlConversionHandler);

			// Add triggers based on foreign key constraints
			if (createTriggers)
				AddTriggersForForeignKeys(sqliteConnection, ds.Tables, sqlConversionHandler);
		}

		protected override void CopySourceDatabaseRowsToSQLiteDatabase(string sourceConnectionString, string sqlitePath, List<ConversionData.TableSchema> schema, string sqliteDbPassword, SqlConversionHandler sqlConversionHandler)
		{
			throw new NotImplementedException();
		}

		protected override void CopySourceDatabaseRowsToSQLiteDatabase(string sourceConnectionString, System.Data.SQLite.SQLiteConnection sqliteConnection, List<ConversionData.TableSchema> schema, SqlConversionHandler handler)
		{
			throw new NotImplementedException();
		}

		protected override void CreateForeignKeySchema(System.Data.IDbConnection sourceConnection, ConversionData.TableSchema tableSchema)
		{
			throw new NotImplementedException();
		}

		protected override ConversionData.TableSchema CreateTableSchema(System.Data.IDbConnection sourceConnection, string tableName, string tableSchemaName)
		{
			throw new NotImplementedException();
		}

		protected override ConversionData.DatabaseSchema ReadSourceSchema(string db2ConnectionString, 
			SqlConversionHandler sqlConversionHandler, SqlTableSelectionHandler sqlTableSelectionHandler)
		{
			Logging.Log(LogLevel.Debug, string.Format("DB2 Connection string: {0}", db2ConnectionString));

			// First step is to read the names of all tables in the database
			List<TableSchema> tables = new List<TableSchema>();
						
			using (iDB2Connection conn = new iDB2Connection(db2ConnectionString))
			{
				conn.Open();

				Logging.Log(LogLevel.Debug, "DB2 Connection initialized");

				List<string> tableNames = new List<string>();
				List<string> tblschema = new List<string>();

				// On DB2 it appears to be case sensitive... 
				Logging.Log(LogLevel.Debug, "Intializing schemas and tables to load where clauses");
				string schemasToLoad = SchemasToLoad.Count > 0 ? Utilities.ConvertListToInClause(SchemasToLoad, "TABLE_SCHEMA").ToUpper() : string.Empty;
				if (schemasToLoad != string.Empty) Logging.Log(LogLevel.Debug, string.Format("Schemas IN clause: {0}", schemasToLoad));
				string tablesToLoad = TablesToLoad.Count > 0 ? Utilities.ConvertListToInClause(TablesToLoad, "TABLE_NAME").ToUpper() : string.Empty;
				if (tablesToLoad != string.Empty) Logging.Log(LogLevel.Debug, string.Format("Tables IN clause: {0}", tablesToLoad));

				string whereClause = string.Empty;
				if (schemasToLoad != string.Empty && tablesToLoad != string.Empty)
					whereClause = string.Format(" AND {0} AND {1} ", schemasToLoad, tablesToLoad);
				else if (schemasToLoad != string.Empty && tablesToLoad == string.Empty)
					whereClause = string.Format(" AND {0} ", schemasToLoad);
				else if (schemasToLoad == string.Empty && tablesToLoad != string.Empty)
					whereClause = string.Format(" AND {0} ", tablesToLoad);

				Logging.Log(LogLevel.Debug, "Intializing SQL statement");
				// This command will read the names of all tables in the database
				string sqlQuery = string.Format(@"select * from SYSTABLES  where TABLE_TYPE = 'T'{0}", whereClause);
				Logging.Log(LogLevel.Debug, string.Format("DB2 SYSTABLE query: \n\n{0}\n\n", sqlQuery));
								
				iDB2Command cmd = new iDB2Command(sqlQuery, conn);				
				using (iDB2DataReader reader = cmd.ExecuteReader())
				{					
					while (reader.Read())
					{						
						tableNames.Add((string)reader["TABLE_NAME"]);
						tblschema.Add((string)reader["TABLE_SCHEMA"]);
					} // while					
				} // using
								
				// Next step is to use ADO APIs to query the schema of each table.
				int count = 0;
				for (int i = 0; i < tableNames.Count; i++)
				{
					string tname = tableNames[i];
					// Load only the tables in TablesToLoad.
					if (TablesToLoad.Count > 0 && !TablesToLoad.Contains(tname.ToLower())) continue;

					string tschma = tblschema[i];
					TableSchema ts = CreateTableSchema(conn, tname, tschma);
					CreateForeignKeySchema(conn, ts);
					tables.Add(ts);
					count++;
					CheckCancelled();
					sqlConversionHandler(false, true, (int)(count * 50.0 / tableNames.Count), "Parsed table " + tname);

					Logging.Log(LogLevel.Debug, "parsed table schema for [" + tname + "]");
				} // foreach
			} // using

			Logging.Log(LogLevel.Debug, "finished parsing all tables in SQL Server schema");

			// Allow the user a chance to select which tables to convert, only if the TablesToLoad list isn't defined.
			if (sqlTableSelectionHandler != null && TablesToLoad.Count > 0)
			{
				List<TableSchema> updated = sqlTableSelectionHandler(tables);
				if (updated != null)
					tables = updated;
			} // if

			Regex removedbo = new Regex(@"dbo\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			// Continue and read all of the views in the database
			List<ViewSchema> views = new List<ViewSchema>();
			using (iDB2Connection conn = new iDB2Connection(db2ConnectionString))
			{
				conn.Open();

				iDB2Command cmd = new iDB2Command(@"SELECT TABLE_NAME, VIEW_DEFINITION  from INFORMATION_SCHEMA.VIEWS", conn);
				using (iDB2DataReader reader = cmd.ExecuteReader())
				{
					int count = 0;
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
						sqlConversionHandler(false, true, 50 + (int)(count * 50.0 / views.Count), "Parsed view " + vs.ViewName);

						Logging.Log(LogLevel.Debug, "parsed view schema for [" + vs.ViewName + "]");
					} // while
				} // using

			} // using

			DatabaseSchema ds = new DatabaseSchema();
			ds.Tables = tables;
			ds.Views = views;
			return ds;
		} 
		#endregion
	}
}
