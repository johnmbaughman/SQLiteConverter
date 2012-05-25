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
using System.Data.SQLite;
using SQLiteConversionEngine.Utility;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;

namespace SQLiteConversionEngine.Conversion {

    public abstract class FromSQLiteConversionBase : ConversionBase<SQLite.Database> {
        private SQLiteConnection sqliteConnection = null;
        private string currentCatalogName = string.Empty;
        private string currentTableName = string.Empty;
        private string currentIndexName = string.Empty;
        private string currentViewName = string.Empty;

        public FromSQLiteConversionBase(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings, List<string> schemasToLoad, List<string> tablesToLoad)
            : base(sqliteConnectionStringSettings, otherConnectionStringSettings, schemasToLoad, tablesToLoad) {
            string[] connectionString = sqliteConnectionStringSettings.ConnectionString.Split(';');
            foreach (string param in connectionString) {
                if (param.ToLower().Contains("data source")) {
                    SourceSchema.FileName = param.Split('=')[1];
                    //SourceSchema.Schema = string.Empty;
                    //SourceSchema.Database.Name = SourceSchema.Name;
                    //SourceSchema.Database.Schema = SourceSchema.Schema;
                }
            }
        }

        public override void ConvertToDatabase() {
            //public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
            using (sqliteConnection = new SQLiteConnection(Connections.SQLiteConnection.ConnectionString)) {
                try {
                    sqliteConnection.Open();
                    LoadSchema();
                    sqliteConnection.Close();
                }
                catch (System.Exception) {
                    if (sqliteConnection.State == ConnectionState.Open) {
                        sqliteConnection.Close();
                    }
                    throw;
                }
            }
        }

        protected override void LoadColumns() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Columns, new string[] { currentCatalogName, null, currentTableName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Columns.Add(row["COLUMN_NAME"].ToString(), new SQLite.Column(row));
                }
            }
        }

        protected override void LoadForeignKeys() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.ForeignKeys, new string[] { currentCatalogName, null, currentTableName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].ForeignKeys.Add(row["CONSTRAINT_NAME"].ToString(), new SQLite.ForeignKey(row));
                }
            }
        }

        protected override void LoadIndexColumns() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.IndexColumns, new string[] { currentCatalogName, null, currentTableName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Indexes[currentIndexName].IndexColumns.Add(row["COLUMN_NAME"].ToString(), new SQLite.IndexColumn(row));
                }
            }
        }

        protected override void LoadIndexes() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Indexes, new string[] { currentCatalogName, null, currentTableName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Indexes.Add(row["INDEX_NAME"].ToString(), new SQLite.Index(row));
                }
            }

            foreach (SQLite.Index index in SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Indexes.Values) {
                currentIndexName = index.IndexName;
                LoadIndexColumns();
            }
        }

        protected override void LoadSchema() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Catalogs)) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Catalogs.Add(row["CATALOG_NAME"].ToString(), new SQLite.Catalog(row));
                }
            }

            foreach (SQLite.Catalog catalog in SourceSchema.Catalogs.Values) {
                currentCatalogName = catalog.CatalogName;
                LoadTables();
                LoadViews();
            }
        }

        protected override void LoadTables() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Tables, new string[] { currentCatalogName })) {
                foreach (DataRow row in dataTable.Rows) {
                    if (TablesToLoad.Contains(row["TABLE_NAME"].ToString())) {
                        SourceSchema.Catalogs[currentCatalogName].Tables.Add(row["TABLE_NAME"].ToString(), new SQLite.Table(row));
                    }
                }
            }

            foreach (SQLite.Table table in SourceSchema.Catalogs[currentCatalogName].Tables.Values) {
                currentTableName = table.Name;
                LoadColumns();
                LoadForeignKeys();
                LoadIndexes();
                //    LoadTriggers();
            }
        }

        protected override void LoadTriggers() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Triggers, new string[] { currentCatalogName, null, currentTableName })) {
                foreach (DataRow row in dataTable.Rows) {
                    //SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Triggers.Add(row["TRIGGER_NAME"].ToString(), new Trigger() {
                    //    TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
                    //    TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
                    //    TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
                    //    Name = row["TRIGGER_NAME"] == DBNull.Value ? null : row["TRIGGER_NAME"].ToString(),
                    //    Definition = row["TRIGGER_DEFINITION"] == DBNull.Value ? null : row["TRIGGER_DEFINITION"].ToString()
                    //});
                }
            }
        }

        protected override void LoadViewColumns() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.ViewColumns, new string[] { currentCatalogName, null, currentViewName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Catalogs[currentCatalogName].Views[currentViewName].ViewColumns.Add(row["COLUMN_NAME"].ToString(), new SQLite.ViewColumn(row));
                }
            }
        }

        protected override void LoadViews() {
            using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Views, new string[] { currentCatalogName })) {
                foreach (DataRow row in dataTable.Rows) {
                    SourceSchema.Catalogs[currentCatalogName].Views.Add(row["TABLE_NAME"].ToString(), new SQLite.View(row));
                }
            }

            foreach (SQLite.View view in SourceSchema.Catalogs[currentCatalogName].Views.Values) {
                currentViewName = view.ViewDefinition;
                LoadViewColumns();
            }
        }

        //private void LoadAllDataTypes() {
        //    DataTable dataTable = sqliteConnection.GetSchema("DATATYPES", null);

        //    foreach (DataRow row in dataTable.Rows) {
        //        _dataTypes.Add(new DataTypes() {
        //            TypeName = row["TypeName"] == DBNull.Value ? null : row["TypeName"].ToString(),
        //            ProviderDbType = row["ProviderDbType"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["ProviderDbType"]),
        //            ColumnSize = row["ColumnSize"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["ColumnSize"]),
        //            CreateFormat = row["CreateFormat"] == DBNull.Value ? null : row["CreateFormat"].ToString(),
        //            CreateParameters = row["CreateParameters"] == DBNull.Value ? null : row["CreateParameters"].ToString(),
        //            DataType = row["DataType"] == DBNull.Value ? null : row["DataType"].ToString(),
        //            IsAutoIncrementable = row["IsAutoIncrementable"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsAutoIncrementable"]),
        //            IsBestMatch = row["IsBestMatch"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsBestMatch"]),
        //            IsCaseSensitive = row["IsCaseSensitive"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsCaseSensitive"]),
        //            IsFixedLength = row["IsFixedLength"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsFixedLength"]),
        //            IsFixedPrecisionScale = row["IsFixedPrecisionScale"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsFixedPrecisionScale"]),
        //            IsLong = row["IsLong"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsLong"]),
        //            IsNullable = row["IsNullable"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsNullable"]),
        //            IsSearchable = row["IsSearchable"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsSearchable"]),
        //            IsSearchableWithLike = row["IsSearchableWithLike"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsSearchableWithLike"]),
        //            IsLiteralSupported = row["IsLiteralSupported"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsLiteralSupported"]),
        //            LiteralPrefix = row["LiteralPrefix"] == DBNull.Value ? null : row["LiteralPrefix"].ToString(),
        //            LiteralSuffix = row["LiteralSuffix"] == DBNull.Value ? null : row["LiteralSuffix"].ToString(),
        //            IsUnsigned = row["IsUnsigned"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsUnsigned"]),
        //            MaximumScale = row["MaximumScale"] == DBNull.Value ? new Nullable<short>() : Convert.ToInt16(row["MaximumScale"]),
        //            MinimumScale = row["MinimumScale"] == DBNull.Value ? new Nullable<short>() : Convert.ToInt16(row["MinimumScale"]),
        //            IsConcurrencyType = row["IsConcurrencyType"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IsConcurrencyType"])
        //        });
        //    }
        //}
    }
}