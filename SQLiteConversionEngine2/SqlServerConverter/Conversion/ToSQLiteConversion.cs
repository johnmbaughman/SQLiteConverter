#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// -----------------------------------------------------

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SQLiteConversionEngine.Conversion;
using SQLiteConversionEngine.InformationSchema.SqlServer;
using SQLiteConversionEngine.Utility;

namespace SqlServerConverter.Conversion {
	internal class ToSQLiteConversion : ToSQLiteConversionBase<Database> {
		private SqlConnection sqlConnection = null;
		private string currentSchemaName = string.Empty;
		private string currentViewName = string.Empty;
		private string currentTableName = string.Empty;
		private string currentIndexName = string.Empty;
		private List<string> defaultIgnores = new List<string> { "sysdiagrams" };

		public ToSQLiteConversion(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) : base(sqliteConnectionStringSettings, otherConnectionStringSettings) { }

		public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			using (sqlConnection = new SqlConnection(Connections.OtherConnection.ConnectionString)) {
				try {
					sqlConnection.Open();
					LoadSchema();
					sqlConnection.Close();
				}
				catch (Exception) {
					if (sqlConnection.State == ConnectionState.Open) {
						sqlConnection.Close();
					}
					throw;
				}
			}
		}

		protected override void ConvertSourceDatabaseToDestination(ConversionBase<Database>.ConversionHandler conversionHandler, ConversionBase<Database>.TableSelectionHandler tableSelectionHandler, ConversionBase<Database>.FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			throw new NotImplementedException();
		}

		protected override void CopySourceDataToDestination(ConversionBase<Database>.ConversionHandler conversionHandler) {
			throw new NotImplementedException();
		}

		protected override void LoadColumns() {
			using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Columns, new string[] { null, currentSchemaName, currentTableName, null })) {
				foreach (DataRow row in dataTable.Rows) {
					Column column = new Column(row);
					SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Columns.Add(column.ColumnName, column);
				}
			}
		}

		protected override void LoadForeignKeys() {
			using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.ForeignKeys, new string[] { null, currentSchemaName, currentTableName, null })) {
				foreach (DataRow row in dataTable.Rows) {
					ForeignKey foreignKey = new ForeignKey();
					foreignKey.LoadItem(row);
					SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].ForeignKeys.Add(foreignKey.ConstraintName, foreignKey);
				}
			}
		}

		protected override void LoadIndexColumns() {
			using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, new string[] { null, currentSchemaName, currentTableName })) {
				foreach (DataRow row in dataTable.Rows) {
					IndexColumn indexColumn = new IndexColumn(row);
					SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes[currentIndexName].IndexColumns.Add(indexColumn.ColumnName, indexColumn);
				}
			}
		}

		protected override void LoadIndexes() {
			using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Indexes, new string[] { null, currentSchemaName, currentTableName })) {
				foreach (DataRow row in dataTable.Rows) {
					Index index = new Index(row);
					SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes.Add(index.IndexName, index);
				}
			}

			foreach (Index catalog in SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes.Values) {
				currentIndexName = catalog.IndexName;
				LoadIndexColumns();
			}
		}

		protected override void LoadSchema() {
			string sql = "SELECT [CATALOG_NAME] ,[SCHEMA_NAME] ,[SCHEMA_OWNER] ,[DEFAULT_CHARACTER_SET_CATALOG] ,[DEFAULT_CHARACTER_SET_SCHEMA] ,[DEFAULT_CHARACTER_SET_NAME] FROM [INFORMATION_SCHEMA].[SCHEMATA]{0}";
			string where = string.Format(" {0}{1}", SchemasToLoad.Count > 0 ? "where" : string.Empty, Utilities.ConvertListToInClause(SchemasToLoad, "SCHEMA_NAME"));

			using (SqlCommand sqlCommand = new SqlCommand(string.Format(sql, where), sqlConnection)) {
				using (SqlDataReader reader = sqlCommand.ExecuteReader()) {
					while (reader.Read()) {
						//SourceSchema.Schemas.Add(reader["SCHEMA_NAME"].ToString(), new Schemata {
						//    CatalogName = reader["CATALOG_NAME"] == DBNull.Value ? null : reader["CATALOG_NAME"].ToString(),
						//    SchemaName = reader["SCHEMA_NAME"] == DBNull.Value ? null : reader["SCHEMA_NAME"].ToString(),
						//    SchemaOwner = reader["SCHEMA_OWNER"] == DBNull.Value ? null : reader["SCHEMA_OWNER"].ToString(),
						//    DefaultCharacterSetCatalog = reader["DEFAULT_CHARACTER_SET_CATALOG"] == DBNull.Value ? null : reader["DEFAULT_CHARACTER_SET_CATALOG"].ToString(),
						//    DefaultCharacterSetSchema = reader["DEFAULT_CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : reader["DEFAULT_CHARACTER_SET_SCHEMA"].ToString(),
						//    DefaultCharacterSetName = reader["DEFAULT_CHARACTER_SET_NAME"] == DBNull.Value ? null : reader["DEFAULT_CHARACTER_SET_NAME"].ToString(),
						//});
					}
				}
			}

			foreach (Schemata catalog in SourceSchema.Schemas.Values) {
				currentSchemaName = catalog.SchemaName;
				LoadTables();
				LoadViews();
			}
		}

		protected override void LoadTables() {
			using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Tables, new string[] { null, currentSchemaName, null, "BASE TABLE" })) {
				foreach (DataRow row in dataTable.Rows) {
					if (!defaultIgnores.Contains(row["TABLE_NAME"].ToString())) {
						Schemata schema = SourceSchema.Schemas[currentSchemaName];
						Table table = new Table(row);
						schema.Tables.Add(table.TableName, table);
					}
				}
			}

			foreach (Table table in SourceSchema.Schemas[currentSchemaName].Tables.Values) {
				currentTableName = table.TableName;
				LoadColumns();
				LoadForeignKeys();
				LoadIndexes();
				//LoadTriggers();
			}
		}

		protected override void LoadTriggers() {
			throw new NotImplementedException();
		}

		protected override void LoadViewColumns() {
			using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.ViewColumns, new string[] { null, currentSchemaName, currentViewName, null })) {
				foreach (DataRow row in dataTable.Rows) {
					ViewColumn viewColumn = new ViewColumn(row);
					SourceSchema.Schemas[currentSchemaName].Views[currentViewName].ViewColumns.Add(viewColumn.ColumnName, viewColumn);
				}
			}
		}

		protected override void LoadViews() {
			using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Views, new string[] { null, currentSchemaName, null })) {
				foreach (DataRow row in dataTable.Rows) {
					View view = new View(row);
					SourceSchema.Schemas[currentSchemaName].Views.Add(view.TableName, view);

					foreach (View catalog in SourceSchema.Schemas[currentSchemaName].Views.Values) {
						currentViewName = catalog.TableName;
						LoadViewColumns();
					}
				}
			}
		}
	}
}