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
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using SQLiteConversionEngine.InformationSchema.SQLite;
using SQLiteConversionEngine.Utility;

namespace SQLiteConversionEngine.Conversion {
	public abstract class FromSQLiteConversionBase<O> : ConversionBase<Database<O>> {
		private SQLiteConnection sqliteConnection = null;
		private string currentCatalogName = string.Empty;
		private string currentTableName = string.Empty;
		private string currentIndexName = string.Empty;
		private string currentViewName = string.Empty;

		public FromSQLiteConversionBase(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings)
			: base(sqliteConnectionStringSettings, otherConnectionStringSettings) {
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

		public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			using (sqliteConnection = new SQLiteConnection(Connections.SQLiteConnection.ConnectionString)) {
				try {
					sqliteConnection.Open();
					LoadSchema();
					sqliteConnection.Close();
				}
				catch (Exception) {
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
					//SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Columns.Add(row["COLUMN_NAME"].ToString(), new Column() {
					//    TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
					//    TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
					//    TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
					//    ColumnName = row["COLUMN_NAME"] == DBNull.Value ? null : row["COLUMN_NAME"].ToString(),
					//    //TODO: read up on BinaryGUID=False in connectionstring
					//    ColumnGuid = row["COLUMN_GUID"] == DBNull.Value ? new Nullable<Guid>() : new Guid(row["COLUMN_GUID"].ToString()),
					//    ColumnPropId = row["COLUMN_PROPID"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["COLUMN_PROPID"]),
					//    OrdinalPosition = row["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["ORDINAL_POSITION"]),
					//    ColumnHasDefault = row["COLUMN_HASDEFAULT"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["COLUMN_HASDEFAULT"].ToString()),
					//    ColumnDefault = row["COLUMN_DEFAULT"] == DBNull.Value ? null : row["COLUMN_DEFAULT"].ToString(),
					//    ColumnFlags = row["COLUMN_FLAGS"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["COLUMN_FLAGS"]),
					//    IsNullable = row["IS_NULLABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_NULLABLE"].ToString()),
					//    DataType = row["DATA_TYPE"] == DBNull.Value ? null : row["DATA_TYPE"].ToString(),
					//    //TODO: read up on BinaryGUID=False in connectionstring
					//    TypeGuid = row["TYPE_GUID"] == DBNull.Value ? new Nullable<Guid>() : new Guid(row["TYPE_GUID"].ToString()),
					//    CharacterMaximumLength = row["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]),
					//    CharacterOctetLength = row["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["CHARACTER_OCTET_LENGTH"]),
					//    NumericPrecision = row["NUMERIC_PRECISION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NUMERIC_PRECISION"]),
					//    NumericScale = row["NUMERIC_SCALE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NUMERIC_SCALE"]),
					//    DateTimePrecision = row["DATETIME_PRECISION"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["DATETIME_PRECISION"]),
					//    CharacterSetCatalog = row["CHARACTER_SET_CATALOG"] == DBNull.Value ? null : row["CHARACTER_SET_CATALOG"].ToString(),
					//    CharacterSetSchema = row["CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : row["CHARACTER_SET_SCHEMA"].ToString(),
					//    CharacterSetName = row["CHARACTER_SET_NAME"] == DBNull.Value ? null : row["CHARACTER_SET_NAME"].ToString(),
					//    CollationCatalog = row["COLLATION_CATALOG"] == DBNull.Value ? null : row["COLLATION_CATALOG"].ToString(),
					//    CollationSchema = row["COLLATION_SCHEMA"] == DBNull.Value ? null : row["COLLATION_SCHEMA"].ToString(),
					//    CollationName = row["COLLATION_NAME"] == DBNull.Value ? null : row["COLLATION_NAME"].ToString(),
					//    DomainCatalog = row["DOMAIN_CATALOG"] == DBNull.Value ? null : row["DOMAIN_CATALOG"].ToString(),
					//    DomainName = row["DOMAIN_NAME"] == DBNull.Value ? null : row["DOMAIN_NAME"].ToString(),
					//    Description = row["DESCRIPTION"] == DBNull.Value ? null : row["DESCRIPTION"].ToString(),
					//    PrimaryKey = row["PRIMARY_KEY"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["PRIMARY_KEY"].ToString()),
					//    EdmType = row["EDM_TYPE"] == DBNull.Value ? null : row["EDM_TYPE"].ToString(),
					//    AutoIncrement = row["AUTOINCREMENT"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["AUTOINCREMENT"].ToString()),
					//    Unique = row["UNIQUE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["UNIQUE"].ToString())
					//});
				}
			}
		}

		protected override void LoadForeignKeys() {
			using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.ForeignKeys, new string[] { currentCatalogName, null, currentTableName })) {
				foreach (DataRow row in dataTable.Rows) {
					//SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].ForeignKeys.Add(row["CONSTRAINT_NAME"].ToString(), new ForeignKey() {
					//    ConstraintCatalog = row["CONSTRAINT_CATALOG"] == DBNull.Value ? null : row["CONSTRAINT_CATALOG"].ToString(),
					//    ConstraintSchema = row["CONSTRAINT_SCHEMA"] == DBNull.Value ? null : row["CONSTRAINT_SCHEMA"].ToString(),
					//    ConstraintName = row["CONSTRAINT_NAME"] == DBNull.Value ? null : row["CONSTRAINT_NAME"].ToString(),
					//    TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
					//    TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
					//    TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
					//    ConstraintType = row["CONSTRAINT_TYPE"] == DBNull.Value ? null : row["CONSTRAINT_TYPE"].ToString(),
					//    IsDeferrable = row["IS_DEFERRABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_DEFERRABLE"].ToString()),
					//    InitiallyDeferred = row["INITIALLY_DEFERRED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["INITIALLY_DEFERRED"].ToString()),
					//    FKeyFromColumn = row["FKEY_FROM_COLUMN"] == DBNull.Value ? null : row["FKEY_FROM_COLUMN"].ToString(),
					//    FKeyFromOrdinalPosition = row["FKEY_FROM_ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["FKEY_FROM_ORDINAL_POSITION"]),
					//    FKeyToCatalog = row["FKEY_TO_CATALOG"] == DBNull.Value ? null : row["FKEY_TO_CATALOG"].ToString(),
					//    FKeyToSchema = row["FKEY_TO_SCHEMA"] == DBNull.Value ? null : row["FKEY_TO_SCHEMA"].ToString(),
					//    FKeyToTable = row["FKEY_TO_TABLE"] == DBNull.Value ? null : row["FKEY_TO_TABLE"].ToString(),
					//    FKeyToColumn = row["FKEY_TO_COLUMN"] == DBNull.Value ? null : row["FKEY_TO_COLUMN"].ToString(),
					//    FKeyOnUpdate = row["FKEY_ON_UPDATE"] == DBNull.Value ? null : row["FKEY_ON_UPDATE"].ToString(),
					//    FKeyOnDelete = row["FKEY_ON_DELETE"] == DBNull.Value ? null : row["FKEY_ON_DELETE"].ToString(),
					//    FKeyMatch = row["FKEY_MATCH"] == DBNull.Value ? null : row["FKEY_MATCH"].ToString()
					//});
				}
			}
		}

		protected override void LoadIndexColumns() {
			using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.IndexColumns, new string[] { currentCatalogName, null, currentTableName })) {
				foreach (DataRow row in dataTable.Rows) {
					//SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Indexes[currentIndexName].IndexColumns.Add(row["COLUMN_NAME"].ToString(), new IndexColumn() {
					//    ConstraintCatalog = row["CONSTRAINT_CATALOG"] == DBNull.Value ? null : row["CONSTRAINT_CATALOG"].ToString(),
					//    ConstraintSchema = row["CONSTRAINT_SCHEMA"] == DBNull.Value ? null : row["CONSTRAINT_SCHEMA"].ToString(),
					//    ConstraintName = row["CONSTRAINT_NAME"] == DBNull.Value ? null : row["CONSTRAINT_NAME"].ToString(),
					//    TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
					//    TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
					//    TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
					//    ColumnName = row["COLUMN_NAME"] == DBNull.Value ? null : row["COLUMN_NAME"].ToString(),
					//    OrdinalPosition = row["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["ORDINAL_POSITION"]),
					//    IndexName = row["INDEX_NAME"] == DBNull.Value ? null : row["INDEX_NAME"].ToString(),
					//    CollationName = row["COLLATION_NAME"] == DBNull.Value ? null : row["COLLATION_NAME"].ToString(),
					//    SortMode = row["SORT_MODE"] == DBNull.Value ? null : row["SORT_MODE"].ToString(),
					//    ConflictOption = row["CONFLICT_OPTION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["CONFLICT_OPTION"])
					//});
				}
			}
		}

		protected override void LoadIndexes() {
			using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Indexes, new string[] { currentCatalogName, null, currentTableName })) {
				foreach (DataRow row in dataTable.Rows) {
					//SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Indexes.Add(row["INDEX_NAME"].ToString(), new Index() {
					//    TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
					//    TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
					//    TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
					//    IndexCatalog = row["INDEX_CATALOG"] == DBNull.Value ? null : row["INDEX_CATALOG"].ToString(),
					//    IndexSchema = row["INDEX_SCHEMA"] == DBNull.Value ? null : row["INDEX_SCHEMA"].ToString(),
					//    IndexName = row["INDEX_NAME"] == DBNull.Value ? null : row["INDEX_NAME"].ToString(),
					//    PrimaryKey = row["PRIMARY_KEY"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["PRIMARY_KEY"].ToString()),
					//    Unique = row["UNIQUE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["UNIQUE"].ToString()),
					//    Clustered = row["CLUSTERED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["CLUSTERED"].ToString()),
					//    Type = row["TYPE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["TYPE"]),
					//    FillFactor = row["FILL_FACTOR"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["FILL_FACTOR"]),
					//    InitialSize = row["INITIAL_SIZE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["INITIAL_SIZE"]),
					//    Nulls = row["NULLS"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NULLS"]),
					//    SortBookmarks = row["SORT_BOOKMARKS"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["SORT_BOOKMARKS"].ToString()),
					//    AutoUpdate = row["AUTO_UPDATE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["AUTO_UPDATE"].ToString()),
					//    NullCollation = row["NULL_COLLATION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NULL_COLLATION"]),
					//    OrdinalPosition = row["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["ORDINAL_POSITION"]),
					//    ColumnName = row["COLUMN_NAME"] == DBNull.Value ? null : row["COLUMN_NAME"].ToString(),
					//    //TODO: read up on BinaryGUID=False in connectionstring
					//    ColumnGUID = row["COLUMN_GUID"] == DBNull.Value ? new Nullable<Guid>() : new Guid(row["COLUMN_GUID"].ToString()),
					//    ColumnPropId = row["COLUMN_PROPID"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["COLUMN_PROPID"]),
					//    Collation = row["COLLATION"] == DBNull.Value ? new Nullable<short>() : Convert.ToInt16(row["COLLATION"]),
					//    Cardinality = row["CARDINALITY"] == DBNull.Value ? new Nullable<decimal>() : Convert.ToDecimal(row["CARDINALITY"]),
					//    Pages = row["PAGES"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["PAGES"]),
					//    FilterCondition = row["FILTER_CONDITION"] == DBNull.Value ? null : row["FILTER_CONDITION"].ToString(),
					//    Integrated = row["INTEGRATED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["INTEGRATED"].ToString()),
					//    IndexDefinition = row["INDEX_DEFINITION"] == DBNull.Value ? null : row["INDEX_DEFINITION"].ToString()
					//});
				}
			}

			//foreach (Index index in SourceSchema.Catalogs[currentCatalogName].Tables[currentTableName].Indexes.Values) {
			//    currentIndexName = index.IndexName;
			//    LoadIndexColumns();
			//}
		}

		protected override void LoadSchema() {
			using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Catalogs)) {
				foreach (DataRow row in dataTable.Rows) {
					//SourceSchema.Catalogs.Add(row["CATALOG_NAME"].ToString(), new Catalog() {
					//    CatalogName = row["CATALOG_NAME"] == DBNull.Value ? null : row["CATALOG_NAME"].ToString(),
					//    Description = row["DESCRIPTION"] == DBNull.Value ? null : row["DESCRIPTION"].ToString(),
					//    Id = row["ID"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["ID"])
					//});
				}
			}

			//foreach (Catalog catalog in SourceSchema.Catalogs.Values) {
			//    currentCatalogName = catalog.CatalogName;
			//    LoadTables();
			//    LoadViews();
			//}
		}

		protected override void LoadTables() {
			using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Tables, new string[] { currentCatalogName })) {
				foreach (DataRow row in dataTable.Rows) {
					//SourceSchema.Catalogs[currentCatalogName].Tables.Add(row["TABLE_NAME"].ToString(), new Table() {
					//    CatalogName = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
					//    Name = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
					//    Type = row["TABLE_TYPE"] == DBNull.Value ? null : row["TABLE_TYPE"].ToString(),
					//    Id = row["TABLE_ID"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["TABLE_ID"]),
					//    RootPage = row["TABLE_ROOTPAGE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["TABLE_ROOTPAGE"]),
					//    Definition = row["TABLE_DEFINITION"] == DBNull.Value ? null : row["TABLE_DEFINITION"].ToString()
					//});
				}
			}

			//foreach (Table table in SourceSchema.Catalogs[currentCatalogName].Tables.Values) {
			//    currentTableName = table.Name;
			//    LoadColumns();
			//    LoadForeignKeys();
			//    LoadIndexes();
			//    LoadTriggers();
			//}
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
					//SourceSchema.Catalogs[currentCatalogName].Views[currentViewName].ViewColumns.Add(row["COLUMN_NAME"].ToString(), new ViewColumn() {
					//    ViewCatalog = row["VIEW_CATALOG"] == DBNull.Value ? null : row["VIEW_CATALOG"].ToString(),
					//    ViewSchema = row["VIEW_SCHEMA"] == DBNull.Value ? null : row["VIEW_SCHEMA"].ToString(),
					//    ViewName = row["VIEW_NAME"] == DBNull.Value ? null : row["VIEW_NAME"].ToString(),
					//    ViewColumnName = row["VIEW_COLUMN_NAME"] == DBNull.Value ? null : row["VIEW_COLUMN_NAME"].ToString(),
					//    TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
					//    TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
					//    TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
					//    ColumnName = row["COLUMN_NAME"] == DBNull.Value ? null : row["COLUMN_NAME"].ToString(),
					//    OrdinalPosition = row["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["ORDINAL_POSITION"]),
					//    ColumnHasDefault = row["COLUMN_HASDEFAULT"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["COLUMN_HASDEFAULT"].ToString()),
					//    ColumnDefault = row["COLUMN_DEFAULT"] == DBNull.Value ? null : row["COLUMN_DEFAULT"].ToString(),
					//    ColumnFlags = row["COLUMN_FLAGS"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["COLUMN_FLAGS"]),
					//    IsNullable = row["IS_NULLABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_NULLABLE"].ToString()),
					//    DataType = row["DATA_TYPE"] == DBNull.Value ? null : row["DATA_TYPE"].ToString(),
					//    CharacterMaximumLength = row["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]),
					//    NumericPrecision = row["NUMERIC_PRECISION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NUMERIC_PRECISION"]),
					//    NumericScale = row["NUMERIC_SCALE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NUMERIC_SCALE"]),
					//    DateTimePrecision = row["DATETIME_PRECISION"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(row["DATETIME_PRECISION"]),
					//    CharacterSetCatalog = row["CHARACTER_SET_CATALOG"] == DBNull.Value ? null : row["CHARACTER_SET_CATALOG"].ToString(),
					//    CharacterSetSchema = row["CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : row["CHARACTER_SET_SCHEMA"].ToString(),
					//    CharacterSetName = row["CHARACTER_SET_NAME"] == DBNull.Value ? null : row["CHARACTER_SET_NAME"].ToString(),
					//    CollationCatalog = row["COLLATION_CATALOG"] == DBNull.Value ? null : row["COLLATION_CATALOG"].ToString(),
					//    CollationSchema = row["COLLATION_SCHEMA"] == DBNull.Value ? null : row["COLLATION_SCHEMA"].ToString(),
					//    CollationName = row["COLLATION_NAME"] == DBNull.Value ? null : row["COLLATION_NAME"].ToString(),
					//    PrimaryKey = row["PRIMARY_KEY"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["PRIMARY_KEY"].ToString()),
					//    EdmType = row["EDM_TYPE"] == DBNull.Value ? null : row["EDM_TYPE"].ToString(),
					//    AutoIncrement = row["AUTOINCREMENT"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["AUTOINCREMENT"].ToString()),
					//    Unique = row["UNIQUE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["UNIQUE"].ToString())
					//});
				}
			}
		}

		protected override void LoadViews() {
			using (DataTable dataTable = sqliteConnection.GetSchema(SQLiteMetaDataCollectionNames.Views, new string[] { currentCatalogName })) {
				foreach (DataRow row in dataTable.Rows) {
					//SourceSchema.Catalogs[currentCatalogName].Views.Add(row["TABLE_NAME"].ToString(), new View() {
					//    TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
					//    TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
					//    TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
					//    ViewDefinition = row["VIEW_DEFINITION"] == DBNull.Value ? null : row["VIEW_DEFINITION"].ToString(),
					//    CheckOption = row["CHECK_OPTION"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["CHECK_OPTION"].ToString()),
					//    IsUpdatable = row["IS_UPDATABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_UPDATABLE"].ToString()),
					//    Description = row["DESCRIPTION"] == DBNull.Value ? null : row["DESCRIPTION"].ToString(),
					//    DateCreated = row["DATE_CREATED"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(row["DATE_CREATED"]),
					//    DateModified = row["DATE_MODIFIED"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(row["DATE_MODIFIED"])
					//});
				}
			}

			//foreach (View view in SourceSchema.Catalogs[currentCatalogName].Views.Values) {
			//    currentViewName = view.ViewDefinition;
			//    LoadViewColumns();
			//}
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