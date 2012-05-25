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
using SQLiteConversionEngine.Utility;
using SqlServer = SQLiteConversionEngine.InformationSchema.SqlServer;

namespace SqlServerConverter.Conversion {

    internal class ToSQLiteConversion : ToSQLiteConversionBase<SqlServer.Database> {
        private SqlConnection sqlConnection = null;
        private string currentSchemaName = string.Empty;
        private string currentViewName = string.Empty;
        private string currentTableName = string.Empty;
        private string currentIndexName = string.Empty;
        private List<string> defaultIgnores = new List<string> { "sysdiagrams" };

        public ToSQLiteConversion(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings, List<string> schemasToLoad, List<string> tablesToLoad)
            : base(sqliteConnectionStringSettings, otherConnectionStringSettings, schemasToLoad, tablesToLoad) { }

        public override void ConvertToDatabase() {
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

        //public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
        //
        //}

        //protected override void ConvertSourceDatabaseToDestination(ConversionBase.ConversionHandler conversionHandler, ConversionBase<Database>.TableSelectionHandler tableSelectionHandler, ConversionBase<Database>.FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
        //    throw new NotImplementedException();
        //}

        //protected override void CopySourceDataToDestination(ConversionBase.ConversionHandler conversionHandler) {
        //    throw new NotImplementedException();
        //}

        protected override void LoadColumns() {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Columns, new string[] { null, currentSchemaName, currentTableName, null })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Columns.Add(row["COLUMN_NAME"].ToString(), new SqlServer.Column(row));
                }
            }
        }

        protected override void LoadForeignKeys() {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.ForeignKeys, new string[] { null, currentSchemaName, currentTableName, null })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].ForeignKeys.Add(row["CONSTRAINT_NAME"].ToString(), new SqlServer.ForeignKey(row));
                }
            }
        }

        protected override void LoadIndexColumns() {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, new string[] { null, currentSchemaName, currentTableName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes[currentIndexName].IndexColumns.Add(row["COLUMN_NAME"].ToString(), new SqlServer.IndexColumn(row));
                }
            }
        }

        protected override void LoadIndexes() {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Indexes, new string[] { null, currentSchemaName, currentTableName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes.Add(row["index_name"].ToString(), new SqlServer.Index(row));
                }
            }

            foreach (SqlServer.Index catalog in SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes.Values) {
                currentIndexName = catalog.IndexName;
                LoadIndexColumns();
            }
        }

        protected override void LoadSchema() {
            string sql = "SELECT [CATALOG_NAME] ,[SCHEMA_NAME] ,[SCHEMA_OWNER] ,[DEFAULT_CHARACTER_SET_CATALOG] ,[DEFAULT_CHARACTER_SET_SCHEMA] ,[DEFAULT_CHARACTER_SET_NAME] FROM [INFORMATION_SCHEMA].[SCHEMATA]{0}";
            string where = string.Format(" {0} {1}", SchemasToLoad.Count > 0 ? "where" : string.Empty, Utilities.ConvertListToInClause(SchemasToLoad, "SCHEMA_NAME"));

            using (SqlCommand sqlCommand = new SqlCommand(string.Format(sql, where), sqlConnection)) {
                using (SqlDataReader reader = sqlCommand.ExecuteReader()) {
                    DataTable table = new DataTable();
                    table.Load(reader);
                    foreach (DataRow row in table.Rows) {
                        SourceSchema.Schemas.Add(row["SCHEMA_NAME"].ToString(), new SqlServer.Schemata(row));
                    }
                }
            }

            foreach (SqlServer.Schemata catalog in SourceSchema.Schemas.Values) {
                currentSchemaName = catalog.SchemaName;
                LoadTables();
                LoadViews();
            }
        }

        protected override void LoadTables() {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Tables, new string[] { null, currentSchemaName, null, "BASE TABLE" })) {
                foreach (DataRow row in dataTable.Rows) {
                    if (!defaultIgnores.Contains(row["TABLE_NAME"].ToString())) {
                        if (TablesToLoad.Contains(row["TABLE_NAME"].ToString())) {
                            SourceSchema.Schemas[currentSchemaName].Tables.Add(row["TABLE_NAME"].ToString(), new SqlServer.Table(row));
                        }
                    }
                }
            }

            foreach (SqlServer.Table table in SourceSchema.Schemas[currentSchemaName].Tables.Values) {
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
                    SourceSchema.Schemas[currentSchemaName].Views[currentViewName].ViewColumns.Add(row["COLUMN_NAME"].ToString(), new SqlServer.ViewColumn(row));
                }
            }
        }

        protected override void LoadViews() {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Views, new string[] { null, currentSchemaName, null })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Schemas[currentSchemaName].Views.Add(row["TABLE_NAME"].ToString(), new SqlServer.View(row));
                }

                foreach (SqlServer.View catalog in SourceSchema.Schemas[currentSchemaName].Views.Values) {
                    currentViewName = catalog.TableName;
                    LoadViewColumns();
                }
            }
        }
    }
}