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
using System.Data.SqlClient;
using System.Text;
using Massive;
using SQLiteConversionEngine.Conversion;
using SQLiteConversionEngine.InformationSchema;
using SQLiteConversionEngine.InformationSchema.SqlServer;
using SQLiteConversionEngine.Utility;

namespace SqlServerConverter {
	internal class ToSQLiteConversion : ToSQLiteConversionBase<Database> {
		private SqlConnection sqlConnection = null;

		public ToSQLiteConversion(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) : base(sqliteConnectionStringSettings, otherConnectionStringSettings) { }

		public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			sqlConnection = new SqlConnection(Connections.OtherConnection.ConnectionString);
			try {
				sqlConnection.Open();
				LoadSchema();
				sqlConnection.Close();
			}
			catch (Exception) {
				throw;
			}
		}

		protected override void ConvertSourceDatabaseToDestination(ConversionBase<Database>.ConversionHandler conversionHandler, ConversionBase<Database>.TableSelectionHandler tableSelectionHandler, ConversionBase<Database>.FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			throw new NotImplementedException();
		}

		protected override void CopySourceDataToDestination(ConversionBase<Database>.ConversionHandler conversionHandler) {
			throw new NotImplementedException();
		}

		protected override void LoadColumns() {
			throw new NotImplementedException();
		}

		protected override void LoadForeignKeys() {
			throw new NotImplementedException();
		}

		protected override void LoadIndexColumns() {
			throw new NotImplementedException();
		}

		protected override void LoadIndexes() {
			throw new NotImplementedException();
		}

		protected override void LoadSchema() {
			//DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Databases);
			//foreach (DataRow row in dataTable.Rows) {
			//    SourceSchema.Schemas.Add(new Schemata {
			//        CatalogName = row["CATALOG_NAME"] == DBNull.Value ? null : row["CATALOG_NAME"].ToString(),
			//        SchemaName = row["SCHEMA_NAME"] == DBNull.Value ? null : row["SCHEMA_NAME"].ToString(),
			//        SchemaOwner = row["SCHEMA_OWNER"] == DBNull.Value ? null : row["SCHEMA_OWNER"].ToString(),
			//        DefaultCharacterSetCatalog = row["DEFAULT_CHARACTER_SET_CATALOG"] == DBNull.Value ? null : row["DEFAULT_CHARACTER_SET_CATALOG"].ToString(),
			//        DefaultCharacterSetSchema = row["DEFAULT_CHARACTER_SET_SCHEMA]"] == DBNull.Value ? null : row["DEFAULT_CHARACTER_SET_SCHEMA"].ToString(),
			//        DefaultCharacterSetName = row["DEFAULT_CHARACTER_SET_NAME"] == DBNull.Value ? null : row["DEFAULT_CHARACTER_SET_NAME"].ToString(),
			//    });
			//}

			//foreach (Catalog catalog in SourceSchema.CatalogCollection) {
			//    currentCatalogName = catalog.CatalogName;
			//    LoadTables();
			//    LoadViews();
			//}
		}

		protected override void LoadTables() {
			throw new NotImplementedException();
		}

		protected override void LoadTriggers() {
			throw new NotImplementedException();
		}

		protected override void LoadViewColumns() {
			throw new NotImplementedException();
		}

		protected override void LoadViews() {
			throw new NotImplementedException();
		}
		///// <summary>
		///// Converts to database.
		///// </summary>
		///// <param name="conversionHandler">The conversion handler.</param>
		///// <param name="tableSelectionHandler">The table selection handler.</param>
		///// <param name="failedViewDefinitionHandler">The failed view definition handler.</param>
		///// <param name="createTriggers">if set to <c>true</c> [create triggers].</param>
		//public override void ConvertToDatabase(ConversionBase.ConversionHandler conversionHandler, ConversionBase.TableSelectionHandler tableSelectionHandler, ConversionBase.FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
		//    ReadSourceSchema(conversionHandler, tableSelectionHandler);
		//}

		///// <summary>
		///// Initializes a new instance of the <see cref="ToSQLiteConversion"/> class.
		///// </summary>
		///// <param name="sqliteConnectionStringSettings">The sqlite connection string settings.</param>
		///// <param name="otherConnectionStringSettings">The other connection string settings.</param>
		//public ToSQLiteConversion(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings)
		//    : base(sqliteConnectionStringSettings, otherConnectionStringSettings) {
		//}

		///// <summary>
		///// Reads the source schema.
		///// </summary>
		///// <param name="conversionHandler">The conversion handler.</param>
		///// <param name="tableSelectionHandler">The table selection handler.</param>
		//protected override void ReadSourceSchema(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler) {
		//    string schemaInClause = SchemasToLoad.Count > 0 ? Utilities.ConvertListToInClause(SchemasToLoad, "TABLE_SCHEMA") : string.Empty;
		//    string tableInClause = TablesToLoad.Count > 0 ? Utilities.ConvertListToInClause(TablesToLoad, "TABLE_NAME") : string.Empty;

		//    string whereClause = string.Empty;
		//    if (schemaInClause != string.Empty && tableInClause != string.Empty)
		//        whereClause = string.Format(" AND {0} AND {1} ", schemaInClause, tableInClause);
		//    else if (schemaInClause != string.Empty && tableInClause == string.Empty)
		//        whereClause = string.Format(" AND {0} ", schemaInClause);
		//    else if (schemaInClause == string.Empty && tableInClause != string.Empty)
		//        whereClause = string.Format(" AND {0} ", tableInClause);

		//    var infoSchema = Massive.DynamicModel.Open(Connections.OtherConnection).Query(string.Format(@"select * from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE = 'BASE TABLE'{0}", whereClause));

		//    foreach (var schema in infoSchema) {
		//        Table table = new Table {
		//            Name = schema.TABLE_NAME,
		//            Schema = schema.TABLE_SCHEMA
		//        };

		//        CreateTableSchema(table);

		//        CreateForeignKeySchema(table);
		//    }
		//}

		//protected override string BuildSourceTableQuery(Table table) {
		//    throw new NotImplementedException();
		//}

		///// <summary>
		///// Creates the table schema.
		///// </summary>
		///// <param name="table">The table.</param>
		//protected override void CreateTableSchema(Table table) {
		//    StringBuilder query = new StringBuilder();
		//    query.Append("SELECT COLUMN_NAME,COLUMN_DEFAULT,IS_NULLABLE,DATA_TYPE, ");
		//    query.Append(" (columnproperty(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity')) AS [IDENT], ");
		//    query.Append("CHARACTER_MAXIMUM_LENGTH AS CSIZE ");
		//    query.AppendFormat("FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{0}' AND TABLE_NAME = '{1}' ORDER BY ", table.Schema, table.Name);
		//    query.Append("ORDINAL_POSITION ASC");
		//    var tableSchema = Massive.DynamicModel.Open(Connections.OtherConnection).Query(query.ToString());

		//    foreach (var columnSchema in tableSchema) {
		//        if (columnSchema.COLUMN_NAME == null) {
		//            continue;
		//        }

		//        Column column = new Column {
		//            Name = columnSchema.COLUMN_NAME,
		//            DefaultValue = columnSchema.COLUMN_DEFAULT,
		//            IsNullable = (columnSchema.IS_NULLABLE == "YES")
		//        };

		//        table.Columns.Add(column);
		//    }
		//}

		///// <summary>
		///// Creates the foreign key schema.
		///// </summary>
		///// <param name="table">The table.</param>
		//protected override void CreateForeignKeySchema(Table table) {
		//    StringBuilder query = new StringBuilder();
		//    query.Append("SELECT FromColumn = CU.COLUMN_NAME, ForeignKeyTable  = PK.TABLE_NAME, ");
		//    query.Append("  ToColumn = PT.COLUMN_NAME, DeleteRule = C.DELETE_RULE, IsNullable = COL.IS_NULLABLE ");
		//    query.Append("FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C ");
		//    query.Append("INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME ");
		//    query.Append("INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME ");
		//    query.Append("INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME ");
		//    query.Append("INNER JOIN ( SELECT i1.TABLE_NAME, i2.COLUMN_NAME ");
		//    query.Append("FROM  INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1 INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME ");
		//    query.Append("WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' ) PT ON PT.TABLE_NAME = PK.TABLE_NAME ");
		//    query.Append("INNER JOIN INFORMATION_SCHEMA.COLUMNS AS COL ON CU.COLUMN_NAME = COL.COLUMN_NAME AND FK.TABLE_NAME = COL.TABLE_NAME ");
		//    query.AppendFormat("WHERE FK.Table_NAME='{0}'", table.Name);
		//    var fkeySchema = Massive.DynamicModel.Open(Connections.OtherConnection).Query(query.ToString());

		//    foreach (var fkey in fkeySchema) {
		//        ForeignKey foreignKey = new ForeignKey {
		//            ForeignKeyTable = fkey.FForeignKeyTable,
		//            FromColumn = fkey.ColumnName,
		//            OnDelete = fkey.DeleteRule,
		//            Schema = table.Schema,
		//            ToColumn = fkey.ToColumn
		//        };

		//        table.ForeignKeys.Add(foreignKey);
		//    }
		//}
	}
}