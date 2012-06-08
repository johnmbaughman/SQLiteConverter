using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using SQLiteConversionEngine.ConversionData;
using SQLiteConversionEngine.Utility;

namespace SQLiteConversionEngine.Converters {
	/// <summary>
	/// This class is resposible to take a single SQL Server database
	/// and convert it to an SQLite database file.
	/// </summary>
	/// <remarks>The class knows how to convert table and index structures only.</remarks>
	public abstract class BaseConverter {
		#region Private Variables

		protected static bool _isActive = false;
		protected static bool _cancelled = false;
		protected static Regex _keyRx = new Regex(@"([a-zA-Z_0-9]+)(\(\-\))?");
		protected static Regex _defaultValueRx = new Regex(@"\(N(\'.*\')\)");

		#endregion Private Variables

		#region Public Properties

		/// <summary>
		/// Gets a value indicating whether this instance is active.
		/// </summary>
		/// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
		public bool IsActive {
			get { return _isActive; }
		}

		private List<string> schemasToLoad = new List<string>();

		/// <summary>
		/// Gets the schemas to load.
		/// </summary>
		/// <value>The schemas to load.</value>
		protected List<string> SchemasToLoad {
			get {
				return schemasToLoad;
			}
		}

		private List<TableToLoad> tablesToLoad = new List<TableToLoad>();

		/// <summary>
		/// Gets or sets the tables to load.
		/// </summary>
		/// <value>The tables to load.</value>
		public List<TableToLoad> TablesToLoad { get; set; }

		#endregion Public Properties

		#region Abstract Methods

		/// <summary>
		/// This method takes as input the connection string to an SQL Server database
		/// and creates a corresponding SQLite database file with a schema derived from
		/// the SQL Server database.
		/// </summary>
		/// <param name="sqlServerConnectionString">The connection string to the SQL Server database.</param>
		/// <param name="sqliteConnectionString">The connection string to the SQLite database.</param>
		/// <param name="sqlConversionHandler">The SQL conversion handler.</param>
		/// <param name="sqlTableSelecttionHandler">The SQL table selection handler.</param>
		/// <param name="failedViewDefinitionHandler">The failed view definition handler.</param>
		/// <param name="createTriggers">if set to <c>true</c> [create triggers].</param>
		/// <remarks>The method continues asynchronously in the background and the caller returned
		/// immediatly.</remarks>
		public abstract void ConvertToDatabase(string sqlServerConnectionString, string sqliteConnectionString,
			SqlConversionHandler sqlConversionHandler,
			SqlTableSelectionHandler sqlTableSelectionHandler,
			FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers);

		/// <summary>
		/// This method takes as input the connection string to an SQL Server database
		/// and creates a corresponding SQLite database file with a schema derived from
		/// the SQL Server database.
		/// </summary>
		/// <param name="sqlServerConnectionString">The connection string to the SQL Server database.</param>
		/// <param name="sqliteConnection">The path to the SQLite database file that needs to get created.</param>
		/// <param name="sqlConversionHandler">The SQL conversion handler.</param>
		/// <param name="sqlTableSelectionHandler">The SQL table selection handler.</param>
		/// <param name="failedViewDefinitionHandler">The failed view definition handler.</param>
		/// <param name="createTriggers">if set to <c>true</c> [create triggers].</param>
		/// <remarks>The method continues asynchronously in the background and the caller returned
		/// immediatly.</remarks>
		public abstract void ConvertToDatabase(string sqlServerConnectionString,
			SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler,
			SqlTableSelectionHandler sqlTableSelectionHandler,
			FailedViewDefinitionHandler failedViewDefinitionHandler,
			bool createTriggers);

		/// <summary>
		/// This method takes as input the connection to an SQL Server database
		/// and creates a corresponding SQLite database file with a schema derived from
		/// the SQL Server database.
		/// </summary>
		/// <param name="sqlConnection">The SQL connection.</param>
		/// <param name="sqliteConnection">The SQLite connection.</param>
		/// <param name="sqlConversionHandler">The SQL conversion handler.</param>
		/// <param name="sqlTableSelectionHandler">The SQL table selection handler.</param>
		/// <param name="failedViewDefinitionHandler">The failed view definition handler.</param>
		/// <param name="createTriggers">if set to <c>true</c> [create triggers].</param>
		/// <remarks>The method continues asynchronously in the background and the caller returned
		/// immediatly.</remarks>
		public abstract void ConvertToDatabase(SqlConnection sqlConnection,
			SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler,
			SqlTableSelectionHandler sqlTableSelectionHandler,
			FailedViewDefinitionHandler failedViewDefinitionHandler,
			bool createTriggers);

		/// <summary>
		/// Do the entire process of first reading the SQL Server schema, creating a corresponding
		/// SQLite schema, and copying all rows from the SQL Server database to the SQLite database.
		/// </summary>
		/// <param name="sqlConnection">The SQL connection.</param>
		/// <param name="sqliteConnection">The SQLite connection.</param>
		/// <param name="sqlConversionHandler">The SQL conversion handler.</param>
		/// <param name="sqlTableSelectionHandler">The SQL table selection handler.</param>
		/// <param name="failedViewDefinitionHandler">The failed view definition handler.</param>
		/// <param name="createTriggers">if set to <c>true</c> [create triggers].</param>
		protected abstract void ConvertSourceDatabaseToDestination(SqlConnection sqlConnection,
			SQLiteConnection sqliteConnection, SqlConversionHandler sqlConversionHandler,
			SqlTableSelectionHandler sqlTableSelectionHandler,
			FailedViewDefinitionHandler failedViewDefinitionHandler,
			bool createTriggers);

		/// <summary>
		/// Copies table rows from the SQL Server database to the SQLite database.
		/// </summary>
		/// <param name="sqlConnection">The SQL connection.</param>
		/// <param name="sqliteConnection">The SQLite connection.</param>
		/// <param name="schema">The schema of the SQL Server database.</param>
		/// <param name="handler">A handler to handle progress notifications.</param>
		protected abstract void CopySourceDatabaseRowsToDestination(
			SqlConnection sqlConnection, SQLiteConnection sqliteConnection, List<TableSchema> schema,
			SqlConversionHandler handler);

		/// <summary>
		/// Builds a SELECT query for a specific table. Needed in the process of copying rows
		/// from the SQL Server database to the SQLite database.
		/// </summary>
		/// <param name="ts">The table schema of the table for which we need the query.</param>
		/// <returns>The SELECT query for the table.</returns>
		protected abstract string BuildSourceTableQuery(TableSchema tableSchema);

		/// <summary>
		/// Reads the entire SQL Server DB schema using the specified connection string.
		/// </summary>
		/// <param name="sqlConnection">The SQL connection.</param>
		/// <param name="sqlConversionHandler">The SQL conversion handler.</param>
		/// <param name="sqlTableSelectionHandler">The SQL table selection handler.</param>
		/// <returns>
		/// Database schema objects for every table/view in the SQL Server database.
		/// </returns>
		protected abstract DatabaseSchema ReadSourceSchema(SqlConnection sqlConnection, SqlConversionHandler sqlConversionHandler,
			SqlTableSelectionHandler sqlTableSelectionHandler);

		/// <summary>
		/// Creates a TableSchema object using the specified SQL Server connection
		/// and the name of the table for which we need to create the schema.
		/// </summary>
		/// <param name="conn">The SQL Server connection to use</param>
		/// <param name="tableName">The name of the table for which we wants to create the table schema.</param>
		/// <returns>A table schema object that represents our knowledge of the table schema</returns>
		protected abstract TableSchema CreateTableSchema(IDbConnection sourceConnection, string tableName, string tableSchemaName);

		/// <summary>
		/// Add foreign key schema object from the specified components (Read from SQL Server).
		/// </summary>
		/// <param name="conn">The SQL Server connection to use</param>
		/// <param name="ts">The table schema to whom foreign key schema should be added to</param>
		protected abstract void CreateForeignKeySchema(IDbConnection sourceConnection, TableSchema tableSchema);

		#endregion Abstract Methods

		#region Public Methods

		/// <summary>
		/// Cancels the conversion.
		/// </summary>
		public void CancelConversion() {
			_cancelled = true;
		}

		#region SQLite

		public SQLiteConnection InitializeSQLiteConnection(string sqliteConnectionString) {
			SQLiteConnectionStringBuilder constring = new SQLiteConnectionStringBuilder();
			constring.ConnectionString = sqliteConnectionString;
			//Logging.Log(LogLevel.Debug, "SQLite Connection string initialized");

			SQLiteConnection conn = new SQLiteConnection(constring.ToString());
			conn.Open();

			return conn;
		}

		public void InitializeSQLiteDatabase(SQLiteConnection sqliteConnection, List<string[]> PragmaCommands) {
			try {
				//Logging.Log(LogLevel.Debug, "Begin SQLite database initialization");
				//Logging.Log(LogLevel.Debug, "SQLite Connection opened");

				SQLiteCommand com = sqliteConnection.CreateCommand();

				//Logging.Log(LogLevel.Debug, "Begin PRAGMA commands");
				foreach (string[] command in PragmaCommands) {
					//Logging.Log(LogLevel.Debug, string.Format(@"PRAGMA {0} = {1}", command[0], command[1]));
					com.CommandText = string.Format(@"PRAGMA {0} = {1}", command[0], command[1]);
					com.ExecuteNonQuery();
				}

				//Logging.Log(LogLevel.Debug, "End SQLite database initialization");
			}
			catch (Exception ex) {
				//Logging.Log(LogLevel.Fatal, FileLogger.GetInnerException(ex).Message);
				//Logging.HandleException(ex);
				throw;
			}
		}

		#endregion SQLite

		#region Sql Server

		public SqlConnection InitializeSqlServerConnection(string sqlServerConnectionString) {
			SqlConnectionStringBuilder constring = new SqlConnectionStringBuilder();
			constring.ConnectionString = sqlServerConnectionString;
			//Logging.Log(LogLevel.Debug, "SQLite Connection string initialized");

			SqlConnection conn = new SqlConnection(constring.ToString());
			conn.Open();

			return conn;
		}

		public void InitializeSqlServerDatabase(SqlConnection sqlConnection) {
			//try
			//{
			//    //Logging.Log(LogLevel.Debug, "Begin SQLite database initialization");
			//    //Logging.Log(LogLevel.Debug, "SQLite Connection opened");

			//    SQLiteCommand com = sqlConnection.CreateCommand();

			//    //Logging.Log(LogLevel.Debug, "Begin PRAGMA commands");
			//    foreach (string[] command in PragmaCommands)
			//    {
			//        //Logging.Log(LogLevel.Debug, string.Format(@"PRAGMA {0} = {1}", command[0], command[1]));
			//        com.CommandText = string.Format(@"PRAGMA {0} = {1}", command[0], command[1]);
			//        com.ExecuteNonQuery();
			//    }

			//    //Logging.Log(LogLevel.Debug, "End SQLite database initialization");
			//}
			//catch (Exception ex)
			//{
			//    //Logging.Log(LogLevel.Fatal, FileLogger.GetInnerException(ex).Message);
			//    //Logging.HandleException(ex);
			//    throw;
			//}
		}

		#endregion Sql Server

		#endregion Public Methods

		#region Protected Methods

		#region SQLite

		/// <summary>
		/// Creates a command object needed to insert values into a specific SQLite table.
		/// </summary>
		/// <param name="ts">The table schema object for the table.</param>
		/// <returns>A command object with the required functionality.</returns>
		protected static SQLiteCommand BuildSQLiteInsert(TableSchema ts) {
			SQLiteCommand res = new SQLiteCommand();

			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO [" + ts.TableName + "] (");
			for (int i = 0; i < ts.Columns.Count; i++) {
				sb.Append("[" + ts.Columns[i].ColumnName + "]");
				if (i < ts.Columns.Count - 1)
					sb.Append(", ");
			}
			sb.Append(") VALUES (");

			List<string> pnames = new List<string>();
			for (int i = 0; i < ts.Columns.Count; i++) {
				string pname = "@" + GetNormalizedName(ts.Columns[i].ColumnName, pnames);
				sb.Append(pname);
				if (i < ts.Columns.Count - 1)
					sb.Append(", ");

				DbType dbType = GetDbTypeOfColumn(ts.Columns[i]);
				SQLiteParameter prm = new SQLiteParameter(pname, dbType, ts.Columns[i].ColumnName);
				res.Parameters.Add(prm);

				// Remember the parameter name in order to avoid duplicates
				pnames.Add(pname);
			}
			sb.Append(")");
			res.CommandText = sb.ToString();
			res.CommandType = CommandType.Text;
			return res;
		}

		/// <summary>
		/// Creates a command object needed to insert values into a specific SQLite table.
		/// </summary>
		/// <param name="ts">The table schema object for the table.</param>
		/// <returns>A command object with the required functionality.</returns>
		protected static SQLiteCommand BuildSQLiteUpdate(TableSchema ts) {
			// Need primary key for this to work. If no primary key, MUST SKIP.
			SQLiteCommand res = new SQLiteCommand();

			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO [" + ts.TableName + "] (");
			for (int i = 0; i < ts.Columns.Count; i++) {
				sb.Append("[" + ts.Columns[i].ColumnName + "]");
				if (i < ts.Columns.Count - 1)
					sb.Append(", ");
			}
			sb.Append(") VALUES (");

			List<string> pnames = new List<string>();
			for (int i = 0; i < ts.Columns.Count; i++) {
				string pname = "@" + GetNormalizedName(ts.Columns[i].ColumnName, pnames);
				sb.Append(pname);
				if (i < ts.Columns.Count - 1)
					sb.Append(", ");

				DbType dbType = GetDbTypeOfColumn(ts.Columns[i]);
				SQLiteParameter prm = new SQLiteParameter(pname, dbType, ts.Columns[i].ColumnName);
				res.Parameters.Add(prm);

				// Remember the parameter name in order to avoid duplicates
				pnames.Add(pname);
			}
			sb.Append(")");
			res.CommandText = sb.ToString();
			res.CommandType = CommandType.Text;
			return res;
		}

		/// <summary>
		/// Creates the SQLite database from the schema read from the SQL Server.
		/// </summary>
		/// <param name="sqlitePath">The path to the generated DB file.</param>
		/// <param name="schema">The schema of the SQL server database.</param>
		/// <param name="password">The password to use for encrypting the DB or null if non is needed.</param>
		/// <param name="handler">A handle for progress notifications.</param>
		protected static void CreateSQLiteDatabase(string sqlitePath, DatabaseSchema schema, string password,
			SqlConversionHandler handler,
			FailedViewDefinitionHandler viewFailureHandler) {
			//Logging.Log(LogLevel.Debug, "Creating SQLite database...");

			// Create the SQLite database file
			SQLiteConnection.CreateFile(sqlitePath);

			//Logging.Log(LogLevel.Debug, "SQLite file was created successfully at [" + sqlitePath + "]");

			// Connect to the newly created database
			string sqliteConnString = CreateSQLiteConnectionString(sqlitePath, password);
			using (SQLiteConnection conn = new SQLiteConnection(sqliteConnString)) {
				conn.Open();

				// Create all tables in the new database
				int count = 0;
				foreach (TableSchema dt in schema.Tables) {
					try {
						AddSQLiteTable(conn, dt);
					}
					catch (Exception ex) {
						//Logging.Log(LogLevel.Error, string.Format("AddSQLiteTable failed: {0}", FileLogger.GetInnerException(ex).Message));
						//Logging.HandleException(ex);
						throw;
					}
					count++;
					CheckCancelled();
					handler(false, true, (int)(count * 50.0 / schema.Tables.Count), "Added table " + dt.TableName + " to the SQLite database");

					//Logging.Log(LogLevel.Debug, "added schema for SQLite table [" + dt.TableName + "]");
				}

				// Create all views in the new database
				count = 0;
				foreach (ViewSchema vs in schema.Views) {
					try {
						AddSQLiteView(conn, vs, viewFailureHandler);
					}
					catch (Exception ex) {
						//Logging.Log(LogLevel.Error, string.Format("AddSQLiteView failed: {0}", FileLogger.GetInnerException(ex).Message));
						//Logging.HandleException(ex);
						throw;
					}
					count++;
					CheckCancelled();
					handler(false, true, 50 + (int)(count * 50.0 / schema.Views.Count), "Added view " + vs.ViewName + " to the SQLite database");

					//Logging.Log(LogLevel.Debug, "added schema for SQLite view [" + vs.ViewName + "]");
				}
			}

			//Logging.Log(LogLevel.Debug, "finished adding all table/view schemas for SQLite database");
		}

		/// <summary>
		/// Creates the SQLite database.
		/// </summary>
		/// <param name="sqliteConnection">The SQLite connection.</param>
		/// <param name="schema">The schema.</param>
		/// <param name="handler">The handler.</param>
		/// <param name="viewFailureHandler">The view failure handler.</param>
		protected static void CreateSQLiteDatabase(SQLiteConnection sqliteConnection, DatabaseSchema schema,
			SqlConversionHandler handler, FailedViewDefinitionHandler viewFailureHandler) {
			//Logging.Log(LogLevel.Debug, "Creating SQLite database...");

			// Create all tables in the new database
			int count = 0;
			foreach (TableSchema dt in schema.Tables) {
				try {
					AddSQLiteTable(sqliteConnection, dt);
				}
				catch (Exception ex) {
					//Logging.Log(LogLevel.Error, string.Format("AddSQLiteTable failed: {0}", FileLogger.GetInnerException(ex).Message));
					//Logging.HandleException(ex);
					throw;
				}
				count++;
				CheckCancelled();
				handler(false, true, (int)(count * 50.0 / schema.Tables.Count), "Added table " + dt.TableName + " to the SQLite database");

				//Logging.Log(LogLevel.Debug, "added schema for SQLite table [" + dt.TableName + "]");
			}

			// Create all views in the new database
			count = 0;
			foreach (ViewSchema vs in schema.Views) {
				try {
					AddSQLiteView(sqliteConnection, vs, viewFailureHandler);
				}
				catch (Exception ex) {
					//Logging.Log(LogLevel.Error, string.Format("AddSQLiteView failed: {0}", FileLogger.GetInnerException(ex).Message));
					//Logging.HandleException(ex);
					throw;
				}
				count++;
				CheckCancelled();
				handler(false, true, 50 + (int)(count * 50.0 / schema.Views.Count), "Added view " + vs.ViewName + " to the SQLite database");

				//Logging.Log(LogLevel.Debug, "added schema for SQLite view [" + vs.ViewName + "]");
			}

			//Logging.Log(LogLevel.Debug, "finished adding all table/view schemas for SQLite database");
		}

		/// <summary>
		/// Creates SQLite connection string from the specified DB file path.
		/// </summary>
		/// <param name="sqlitePath">The path to the SQLite database file.</param>
		/// <returns>SQLite connection string</returns>
		protected static string CreateSQLiteConnectionString(string sqlitePath, string password) {
			SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
			builder.DataSource = sqlitePath;
			if (password != null)
				builder.Password = password;
			builder.PageSize = 4096;
			builder.UseUTF16Encoding = true;
			string connstring = builder.ConnectionString;

			return connstring;
		}

		#endregion SQLite

		#region Sql Server

		/// <summary>
		/// Creates a command object needed to insert values into a specific SQLite table.
		/// </summary>
		/// <param name="ts">The table schema object for the table.</param>
		/// <returns>A command object with the required functionality.</returns>
		protected static SqlCommand BuildSqlServerInsert(TableSchema ts) {
			SqlCommand res = new SqlCommand();

			StringBuilder sb = new StringBuilder();
			sb.Append("INSERT INTO [" + ts.TableName + "] (");
			for (int i = 0; i < ts.Columns.Count; i++) {
				sb.Append("[" + ts.Columns[i].ColumnName + "]");
				if (i < ts.Columns.Count - 1)
					sb.Append(", ");
			}
			sb.Append(") VALUES (");

			List<string> pnames = new List<string>();
			for (int i = 0; i < ts.Columns.Count; i++) {
				string pname = "@" + GetNormalizedName(ts.Columns[i].ColumnName, pnames);
				sb.Append(pname);
				if (i < ts.Columns.Count - 1)
					sb.Append(", ");

				DbType dbType = GetDbTypeOfColumn(ts.Columns[i]);
				SqlParameter prm = new SqlParameter(pname, (SqlDbType)dbType);
				res.Parameters.Add(prm);

				// Remember the parameter name in order to avoid duplicates
				pnames.Add(pname);
			}
			sb.Append(")");
			res.CommandText = sb.ToString();
			res.CommandType = CommandType.Text;
			return res;
		}

		/// <summary>
		/// Creates the SQLite database from the schema read from the SQL Server.
		/// </summary>
		/// <param name="sqlitePath">The path to the generated DB file.</param>
		/// <param name="schema">The schema of the SQL server database.</param>
		/// <param name="password">The password to use for encrypting the DB or null if non is needed.</param>
		/// <param name="handler">A handle for progress notifications.</param>
		protected static void CreateSqlServerDatabase(string sqlitePath, DatabaseSchema schema, string password,
			SqlConversionHandler handler,
			FailedViewDefinitionHandler viewFailureHandler) {
			//Logging.Log(LogLevel.Debug, "Creating SQLite database...");
			//Logging.Log(LogLevel.Debug, "SQLite file was created successfully at [" + sqlitePath + "]");

			// Connect to the newly created database
			string sqliteConnString = CreateSQLiteConnectionString(sqlitePath, password);
			using (SQLiteConnection conn = new SQLiteConnection(sqliteConnString)) {
				conn.Open();

				// Create all tables in the new database
				int count = 0;
				foreach (TableSchema dt in schema.Tables) {
					try {
						AddSQLiteTable(conn, dt);
					}
					catch (Exception ex) {
						//Logging.Log(LogLevel.Error, string.Format("AddSQLiteTable failed: {0}", FileLogger.GetInnerException(ex).Message));
						//Logging.HandleException(ex);
						throw;
					}
					count++;
					CheckCancelled();
					handler(false, true, (int)(count * 50.0 / schema.Tables.Count), "Added table " + dt.TableName + " to the SQLite database");

					//Logging.Log(LogLevel.Debug, "added schema for SQLite table [" + dt.TableName + "]");
				}

				// Create all views in the new database
				count = 0;
				foreach (ViewSchema vs in schema.Views) {
					try {
						AddSQLiteView(conn, vs, viewFailureHandler);
					}
					catch (Exception ex) {
						//Logging.Log(LogLevel.Error, string.Format("AddSQLiteView failed: {0}", FileLogger.GetInnerException(ex).Message));
						//Logging.HandleException(ex);
						throw;
					}
					count++;
					CheckCancelled();
					handler(false, true, 50 + (int)(count * 50.0 / schema.Views.Count), "Added view " + vs.ViewName + " to the SQLite database");

					//Logging.Log(LogLevel.Debug, "added schema for SQLite view [" + vs.ViewName + "]");
				}
			}

			//Logging.Log(LogLevel.Debug, "finished adding all table/view schemas for SQLite database");
		}

		/// <summary>
		/// Creates the SQLite database.
		/// </summary>
		/// <param name="sqliteConnection">The SQLite connection.</param>
		/// <param name="schema">The schema.</param>
		/// <param name="handler">The handler.</param>
		/// <param name="viewFailureHandler">The view failure handler.</param>
		protected static void CreateSqlServerDatabase(SQLiteConnection sqliteConnection, DatabaseSchema schema,
			SqlConversionHandler handler, FailedViewDefinitionHandler viewFailureHandler) {
			//Logging.Log(LogLevel.Debug, "Creating SQLite database...");

			// Create all tables in the new database
			int count = 0;
			foreach (TableSchema dt in schema.Tables) {
				try {
					AddSQLiteTable(sqliteConnection, dt);
				}
				catch (Exception ex) {
					//Logging.Log(LogLevel.Error, string.Format("AddSQLiteTable failed: {0}", FileLogger.GetInnerException(ex).Message));
					//Logging.HandleException(ex);
					throw;
				}
				count++;
				CheckCancelled();
				handler(false, true, (int)(count * 50.0 / schema.Tables.Count), "Added table " + dt.TableName + " to the SQLite database");

				//Logging.Log(LogLevel.Debug, "added schema for SQLite table [" + dt.TableName + "]");
			}

			// Create all views in the new database
			count = 0;
			foreach (ViewSchema vs in schema.Views) {
				try {
					AddSQLiteView(sqliteConnection, vs, viewFailureHandler);
				}
				catch (Exception ex) {
					//Logging.Log(LogLevel.Error, string.Format("AddSQLiteView failed: {0}", FileLogger.GetInnerException(ex).Message));
					//Logging.HandleException(ex);
					throw;
				}
				count++;
				CheckCancelled();
				handler(false, true, 50 + (int)(count * 50.0 / schema.Views.Count), "Added view " + vs.ViewName + " to the SQLite database");

				//Logging.Log(LogLevel.Debug, "added schema for SQLite view [" + vs.ViewName + "]");
			}

			//Logging.Log(LogLevel.Debug, "finished adding all table/view schemas for SQLite database");
		}

		/// <summary>
		/// Creates SQLite connection string from the specified DB file path.
		/// </summary>
		/// <param name="sqlitePath">The path to the SQLite database file.</param>
		/// <returns>SQLite connection string</returns>
		protected static string CreateSqlServerConnectionString(string sqlitePath, string password) {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
			builder.DataSource = sqlitePath;
			if (password != null)
				builder.Password = password;
			string connstring = builder.ConnectionString;

			return connstring;
		}

		#endregion Sql Server

		/// <summary>
		/// Used in order to adjust the value received from SQL Servr for the SQLite database.
		/// </summary>
		/// <param name="val">The value object</param>
		/// <param name="columnSchema">The corresponding column schema</param>
		/// <returns>SQLite adjusted value.</returns>
		protected static object CastValueForColumn(object val, ColumnSchema columnSchema) {
			if (val is DBNull)
				return null;

			DbType dt = GetDbTypeOfColumn(columnSchema);

			switch (dt) {
				case DbType.Int32:
					if (val is short)
						return (int)(short)val;
					if (val is byte)
						return (int)(byte)val;
					if (val is long)
						return (int)(long)val;
					if (val is decimal)
						return (int)(decimal)val;
					break;

				case DbType.Int16:
					if (val is int)
						return (short)(int)val;
					if (val is byte)
						return (short)(byte)val;
					if (val is long)
						return (short)(long)val;
					if (val is decimal)
						return (short)(decimal)val;
					break;

				case DbType.Int64:
					if (val is int)
						return (long)(int)val;
					if (val is short)
						return (long)(short)val;
					if (val is byte)
						return (long)(byte)val;
					if (val is decimal)
						return (long)(decimal)val;
					break;

				case DbType.Single:
					if (val is double)
						return (float)(double)val;
					if (val is decimal)
						return (float)(decimal)val;
					break;

				case DbType.Double:
					if (val is float)
						return (double)(float)val;
					if (val is double)
						return (double)val;
					if (val is decimal)
						return (double)(decimal)val;
					break;

				case DbType.String:
					if (val is Guid)
						return ((Guid)val).ToString();
					break;

				case DbType.Guid:
					if (val is string)
						return ParseStringAsGuid((string)val);
					if (val is byte[])
						return ParseBlobAsGuid((byte[])val);
					break;

				case DbType.DateTime:
					if (val is System.TimeSpan && columnSchema.ColumnType == "time")
						return DateTime.Parse(val.ToString());
					break;

				case DbType.Binary:
				case DbType.Boolean:
				case DbType.Date:
				case DbType.Time:
				case DbType.DateTime2:
					break;

				default:
					//Logging.Log(LogLevel.Error, "argument exception - illegal database type");
					throw new ArgumentException("Illegal database type [" + Enum.GetName(typeof(DbType), dt) + "]");
			}

			return val;
		}

		/// <summary>
		/// Matches SQL Server types to general DB types
		/// </summary>
		/// <param name="columnSchema">The column schema to use for the match</param>
		/// <returns>The matched DB type</returns>
		protected static DbType GetDbTypeOfColumn(ColumnSchema columnSchema) {
			switch (columnSchema.ColumnType) {
				case "tinyint":
					return DbType.Byte;
				case "int":
					return DbType.Int32;
				case "smallint":
					return DbType.Int16;
				case "bigint":
					return DbType.Int64;
				case "bit":
					return DbType.Boolean;
				case "nvarchar":
				case "varchar":
				case "text":
				case "ntext":
					return DbType.String;
				case "float":
					return DbType.Double;
				case "real":
					return DbType.Single;
				case "blob":
					return DbType.Binary;
				case "numeric":
					return DbType.Double;
				case "timestamp":
				case "datetime":
					return DbType.DateTime;
				case "nchar":
				case "char":
					return DbType.String;
				case "uniqueidentifier":
				case "guid":
					return DbType.Guid;
				case "xml":
					return DbType.String;
				case "sql_variant":
					return DbType.Object;
				case "integer":
					return DbType.Int64;
				case "date":
					return DbType.Date;
				case "datetime2":
					return DbType.DateTime2;
				case "time":
					return DbType.DateTime;
			}

			//Logging.Log(LogLevel.Error, "illegal db type found");
			throw new ApplicationException("Illegal DB type found (" + columnSchema.ColumnType + ")");
		}

		/// <summary>
		/// Used in order to avoid breaking naming rules (e.g., when a table has
		/// a name in SQL Server that cannot be used as a basis for a matching index
		/// name in SQLite).
		/// </summary>
		/// <param name="str">The name to change if necessary</param>
		/// <param name="names">Used to avoid duplicate names</param>
		/// <returns>A normalized name</returns>
		protected static string GetNormalizedName(string str, List<string> names) {
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < str.Length; i++) {
				if (Char.IsLetterOrDigit(str[i]) || str[i] == '_')
					sb.Append(str[i]);
				else
					sb.Append("_");
			}

			// Avoid returning duplicate name
			if (names.Contains(sb.ToString()))
				return GetNormalizedName(sb.ToString() + "_", names);
			else
				return sb.ToString();
		}

		protected static Guid ParseBlobAsGuid(byte[] blob) {
			byte[] data = blob;
			if (blob.Length > 16) {
				data = new byte[16];
				for (int i = 0; i < 16; i++)
					data[i] = blob[i];
			}
			else if (blob.Length < 16) {
				data = new byte[16];
				for (int i = 0; i < blob.Length; i++)
					data[i] = blob[i];
			}

			return new Guid(data);
		}

		protected static Guid ParseStringAsGuid(string str) {
			try {
				return new Guid(str);
			}
			catch {
				return Guid.Empty;
			}
		}

		/// <summary>
		/// Convenience method for checking if the conversion progress needs to be cancelled.
		/// </summary>
		protected static void CheckCancelled() {
			if (_cancelled)
				throw new ApplicationException("User cancelled the conversion");
		}

		/// <summary>
		/// Small validation method to make sure we don't miss anything without getting
		/// an exception.
		/// </summary>
		/// <param name="dataType">The datatype to validate.</param>
		protected static void ValidateDataType(string dataType) {
			if (dataType == "int" || dataType == "smallint" ||
				dataType == "bit" || dataType == "float" ||
				dataType == "real" || dataType == "nvarchar" ||
				dataType == "varchar" || dataType == "timestamp" ||
				dataType == "varbinary" || dataType == "image" ||
				dataType == "text" || dataType == "ntext" ||
				dataType == "bigint" ||
				dataType == "char" || dataType == "numeric" ||
				dataType == "binary" || dataType == "smalldatetime" ||
				dataType == "smallmoney" || dataType == "money" ||
				dataType == "tinyint" || dataType == "uniqueidentifier" ||
				dataType == "xml" || dataType == "sql_variant" ||
				dataType == "decimal" || dataType == "nchar" || dataType == "datetime" ||
				dataType == "datetime2" || dataType == "date" || dataType == "time")
				return;
			throw new ApplicationException("Validation failed for data type [" + dataType + "]");
		}

		/// <summary>
		/// Does some necessary adjustments to a value string that appears in a column DEFAULT
		/// clause.
		/// </summary>
		/// <param name="colDefault">The original default value string (as read from SQL Server).</param>
		/// <returns>Adjusted DEFAULT value string (for SQLite)</returns>
		protected static string FixDefaultValueString(string colDefault) {
			bool replaced = false;
			string res = colDefault.Trim();

			// Find first/last indexes in which to search
			int first = -1;
			int last = -1;
			for (int i = 0; i < res.Length; i++) {
				if (res[i] == '\'' && first == -1)
					first = i;
				if (res[i] == '\'' && first != -1 && i > last)
					last = i;
			}

			if (first != -1 && last > first)
				return res.Substring(first, last - first + 1);

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < res.Length; i++) {
				if (res[i] != '(' && res[i] != ')') {
					sb.Append(res[i]);
					replaced = true;
				}
			}
			if (replaced)
				return "(" + sb.ToString() + ")";
			else
				return sb.ToString();
		}

		/// <summary>
		/// Builds an index schema object from the specified components (Read from SQL Server).
		/// </summary>
		/// <param name="indexName">The name of the index</param>
		/// <param name="desc">The description of the index</param>
		/// <param name="keys">Key columns that are part of the index.</param>
		/// <returns>An index schema object that represents our knowledge of the index</returns>
		protected static IndexSchema BuildIndexSchema(string indexName, string desc, string keys) {
			IndexSchema res = new IndexSchema();
			res.IndexName = indexName;

			// Determine if this is a unique index or not.
			string[] descParts = desc.Split(',');
			foreach (string p in descParts) {
				if (p.Trim().Contains("unique")) {
					res.IsUnique = true;
					break;
				}
			}

			// Get all key names and check if they are ASCENDING or DESCENDING
			res.Columns = new List<IndexColumn>();
			string[] keysParts = keys.Split(',');
			foreach (string p in keysParts) {
				Match m = _keyRx.Match(p);
				if (!m.Success) {
					throw new ApplicationException("Illegal key name [" + p + "] in index [" +
						indexName + "]");
				}

				string key = m.Groups[1].Value;
				IndexColumn ic = new IndexColumn();
				ic.ColumnName = key;
				if (m.Groups[2].Success)
					ic.IsAscending = false;
				else
					ic.IsAscending = true;

				res.Columns.Add(ic);
			}

			return res;
		}

		/// <summary>
		/// More adjustments for the DEFAULT value clause.
		/// </summary>
		/// <param name="val">The value to adjust</param>
		/// <returns>Adjusted DEFAULT value string</returns>
		protected static string AdjustDefaultValue(string val) {
			if (val == null || val == string.Empty)
				return val;

			Match m = _defaultValueRx.Match(val);
			if (m.Success)
				return m.Groups[1].Value;
			return val;
		}

		/// <summary>
		/// Gets a create script for the triggerSchema in SQLite syntax
		/// </summary>
		/// <param name="ts">Trigger to script</param>
		/// <returns>Executable script</returns>
		protected static string WriteTriggerSchema(TriggerSchema ts) {
			return @"CREATE TRIGGER [" + ts.Name + "] " +
				   ts.Type + " " + ts.Event +
				   " ON [" + ts.Table + "] " +
				   "BEGIN " + ts.Body + " END;";
		}

		#endregion Protected Methods

		#region Private Methods

		private static void AddSQLiteView(SQLiteConnection conn, ViewSchema vs, FailedViewDefinitionHandler handler) {
			// Prepare a CREATE VIEW DDL statement
			string stmt = vs.ViewSQL;
			//Logging.Log(LogLevel.Debug, "\n\n" + stmt + "\n\n");

			// Execute the query in order to actually create the view.
			SQLiteTransaction tx = conn.BeginTransaction();
			try {
				SQLiteCommand cmd = new SQLiteCommand(stmt, conn, tx);
				cmd.ExecuteNonQuery();

				tx.Commit();
			}
			catch {
				tx.Rollback();

				if (handler != null) {
					ViewSchema updated = new ViewSchema();
					updated.ViewName = vs.ViewName;
					updated.ViewSQL = vs.ViewSQL;

					// Ask the user to supply the new view definition SQL statement
					string sql = handler(updated);

					if (sql == null)
						return; // Discard the view
					else {
						// Try to re-create the view with the user-supplied view definition SQL
						updated.ViewSQL = sql;
						AddSQLiteView(conn, updated, handler);
					}
				}
				else
					throw;
			}
		}

		/// <summary>
		/// Creates the CREATE TABLE DDL for SQLite and a specific table.
		/// </summary>
		/// <param name="conn">The SQLite connection</param>
		/// <param name="dt">The table schema object for the table to be generated.</param>
		private static void AddSQLiteTable(SQLiteConnection conn, TableSchema dt) {
			// Prepare a CREATE TABLE DDL statement
			string stmt = BuildCreateTableQuery(dt);

			//Logging.Log(LogLevel.Debug, "\n\n" + stmt + "\n\n");

			// Execute the query in order to actually create the table.
			SQLiteCommand cmd = new SQLiteCommand(stmt, conn);
			cmd.ExecuteNonQuery();
		}

		/// <summary>
		/// returns the CREATE TABLE DDL for creating the SQLite table from the specified
		/// table schema object.
		/// </summary>
		/// <param name="ts">The table schema object from which to create the SQL statement.</param>
		/// <returns>CREATE TABLE DDL for the specified table.</returns>
		private static string BuildCreateTableQuery(TableSchema ts) {
			StringBuilder sb = new StringBuilder();

			sb.Append("CREATE TABLE [" + ts.TableName + "] (\n");

			bool pkey = false;
			for (int i = 0; i < ts.Columns.Count; i++) {
				ColumnSchema col = ts.Columns[i];
				string cline = BuildColumnStatement(col, ts, ref pkey);
				sb.Append(cline);
				if (i < ts.Columns.Count - 1)
					sb.Append(",\n");
			}

			// add primary keys...
			if (ts.PrimaryKey != null && ts.PrimaryKey.Count > 0 & !pkey) {
				sb.Append(",\n");
				sb.Append("    PRIMARY KEY (");
				for (int i = 0; i < ts.PrimaryKey.Count; i++) {
					sb.Append("[" + ts.PrimaryKey[i] + "]");
					if (i < ts.PrimaryKey.Count - 1)
						sb.Append(", ");
				}
				sb.Append(")\n");
			}
			else
				sb.Append("\n");

			// add foreign keys...
			if (ts.ForeignKeys.Count > 0) {
				sb.Append(",\n");
				for (int i = 0; i < ts.ForeignKeys.Count; i++) {
					ForeignKeySchema foreignKey = ts.ForeignKeys[i];
					string stmt = string.Format("    FOREIGN KEY ([{0}])\n        REFERENCES [{1}]([{2}])",
								foreignKey.ColumnName, foreignKey.ForeignTableName, foreignKey.ForeignColumnName);

					sb.Append(stmt);
					if (i < ts.ForeignKeys.Count - 1)
						sb.Append(",\n");
				}
			}

			sb.Append("\n");
			sb.Append(");\n");

			// Create any relevant indexes
			if (ts.Indexes != null) {
				for (int i = 0; i < ts.Indexes.Count; i++) {
					string stmt = BuildCreateIndex(ts.TableName, ts.Indexes[i]);
					sb.Append(stmt + ";\n");
				}
			}

			string query = sb.ToString();
			return query;
		}

		/// <summary>
		/// Used when creating the CREATE TABLE DDL. Creates a single row
		/// for the specified column.
		/// </summary>
		/// <param name="col">The column schema</param>
		/// <returns>A single column line to be inserted into the general CREATE TABLE DDL statement</returns>
		private static string BuildColumnStatement(ColumnSchema col, TableSchema ts, ref bool pkey) {
			StringBuilder sb = new StringBuilder();
			sb.Append("\t\"" + col.ColumnName + "\"\t\t");

			// Special treatment for IDENTITY columns
			if (col.IsIdentity) {
				if (ts.PrimaryKey.Count == 1 && (col.ColumnType == "tinyint" || col.ColumnType == "int" || col.ColumnType == "smallint" ||
					col.ColumnType == "bigint" || col.ColumnType == "integer")) {
					sb.Append("integer PRIMARY KEY AUTOINCREMENT");
					pkey = true;
				}
				else
					sb.Append("integer");
			}
			else {
				if (col.ColumnType == "int")
					sb.Append("integer");
				else {
					sb.Append(col.ColumnType);
				}
				if (col.Length > 0)
					sb.Append("(" + col.Length + ")");
			}
			if (!col.IsNullable)
				sb.Append(" NOT NULL");

			if (col.IsCaseSensitivite.HasValue && !col.IsCaseSensitivite.Value)
				sb.Append(" COLLATE NOCASE");

			string defval = StripParens(col.DefaultValue);
			defval = DiscardNational(defval);
			//Logging.Log(LogLevel.Debug, "DEFAULT VALUE BEFORE [" + col.DefaultValue + "] AFTER [" + defval + "]");
			if (defval != string.Empty && defval.ToUpper().Contains("GETDATE")) {
				//Logging.Log(LogLevel.Debug, "converted SQL Server GETDATE() to CURRENT_TIMESTAMP for column [" + col.ColumnName + "]");
				sb.Append(" DEFAULT (CURRENT_TIMESTAMP)");
			}
			else if (defval != string.Empty && IsValidDefaultValue(defval))
				sb.Append(" DEFAULT " + defval);

			return sb.ToString();
		}

		/// <summary>
		/// Creates a CREATE INDEX DDL for the specified table and index schema.
		/// </summary>
		/// <param name="tableName">The name of the indexed table.</param>
		/// <param name="indexSchema">The schema of the index object</param>
		/// <returns>A CREATE INDEX DDL (SQLite format).</returns>
		private static string BuildCreateIndex(string tableName, IndexSchema indexSchema) {
			StringBuilder sb = new StringBuilder();
			sb.Append("CREATE ");
			if (indexSchema.IsUnique)
				sb.Append("UNIQUE ");
			sb.Append("INDEX [" + tableName + "_" + indexSchema.IndexName + "]\n");
			sb.Append("ON [" + tableName + "]\n");
			sb.Append("(");
			for (int i = 0; i < indexSchema.Columns.Count; i++) {
				sb.Append("[" + indexSchema.Columns[i].ColumnName + "]");
				if (!indexSchema.Columns[i].IsAscending)
					sb.Append(" DESC");
				if (i < indexSchema.Columns.Count - 1)
					sb.Append(", ");
			}
			sb.Append(")");

			return sb.ToString();
		}

		/// <summary>
		/// Discards the national prefix if exists (e.g., N'sometext') which is not
		/// supported in SQLite.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		private static string DiscardNational(string value) {
			Regex rx = new Regex(@"N\'([^\']*)\'");
			Match m = rx.Match(value);
			if (m.Success)
				return m.Groups[1].Value;
			else
				return value;
		}

		/// <summary>
		/// Check if the DEFAULT clause is valid by SQLite standards
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private static bool IsValidDefaultValue(string value) {
			if (IsSingleQuoted(value))
				return true;

			double testnum;
			if (!double.TryParse(value, out testnum))
				return false;
			return true;
		}

		private static bool IsSingleQuoted(string value) {
			value = value.Trim();
			if (value.StartsWith("'") && value.EndsWith("'"))
				return true;
			return false;
		}

		/// <summary>
		/// Strip any parentheses from the string.
		/// </summary>
		/// <param name="value">The string to strip</param>
		/// <returns>The stripped string</returns>
		private static string StripParens(string value) {
			Regex rx = new Regex(@"\(([^\)]*)\)");
			Match m = rx.Match(value);
			if (!m.Success)
				return value;
			else
				return StripParens(m.Groups[1].Value);
		}

		#endregion Private Methods

		#region Trigger related

		protected static void AddTriggersForForeignKeys(string sqlitePath, IEnumerable<TableSchema> schema,
			string password, SqlConversionHandler handler) {
			// Connect to the newly created database
			string sqliteConnString = CreateSQLiteConnectionString(sqlitePath, password);
			using (SQLiteConnection conn = new SQLiteConnection(sqliteConnString)) {
				conn.Open();
				// foreach
				foreach (TableSchema dt in schema) {
					try {
						AddTableTriggers(conn, dt);
					}
					catch (Exception ex) {
						//Logging.Log(LogLevel.Error, string.Format("AddTableTriggers failed: {0}", FileLogger.GetInnerException(ex).Message));
						throw;
					}
				}
			}

			//Logging.Log(LogLevel.Debug, "finished adding triggers to schema");
		}

		protected static void AddTriggersForForeignKeys(SQLiteConnection sqliteConnection, IEnumerable<TableSchema> schema,
			SqlConversionHandler handler) {
			// foreach
			foreach (TableSchema dt in schema) {
				try {
					AddTableTriggers(sqliteConnection, dt);
				}
				catch (Exception ex) {
					//Logging.Log(LogLevel.Error, string.Format("AddTableTriggers failed: {0}", FileLogger.GetInnerException(ex).Message));
					throw;
				}
			}

			//Logging.Log(LogLevel.Debug, "finished adding triggers to schema");
		}

		private static void AddTableTriggers(SQLiteConnection conn, TableSchema dt) {
			IList<TriggerSchema> triggers = TriggerBuilder.GetForeignKeyTriggers(dt);
			foreach (TriggerSchema trigger in triggers) {
				SQLiteCommand cmd = new SQLiteCommand(WriteTriggerSchema(trigger), conn);
				cmd.ExecuteNonQuery();
			}
		}

		#endregion Trigger related
	}

	/// <summary>
	/// This handler is called whenever a progress is made in the conversion process.
	/// </summary>
	/// <param name="done">TRUE indicates that the entire conversion process is finished.</param>
	/// <param name="success">TRUE indicates that the current step finished successfully.</param>
	/// <param name="percent">Progress percent (0-100)</param>
	/// <param name="msg">A message that accompanies the progress.</param>
	public delegate void SqlConversionHandler(bool done, bool success, int percent, string msg);

	/// <summary>
	/// This handler allows the user to change which tables get converted from SQL Server
	/// to SQLite.
	/// </summary>
	/// <param name="schema">The original SQL Server DB schema</param>
	/// <returns>The same schema minus any table we don't want to convert.</returns>
	public delegate List<TableSchema> SqlTableSelectionHandler(List<TableSchema> schema);

	/// <summary>
	/// This handler is called in order to handle the case when copying the SQL Server view SQL
	/// statement is not enough and the user needs to either update the view definition himself
	/// or discard the view definition from the generated SQLite database.
	/// </summary>
	/// <param name="vs">The problematic view definition</param>
	/// <returns>The updated view definition, or NULL in case the view should be discarded</returns>
	public delegate string FailedViewDefinitionHandler(ViewSchema vs);
}