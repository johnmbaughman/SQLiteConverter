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
using SQLiteConversionEngine.InformationSchema;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;
using SqlServer = SQLiteConversionEngine.InformationSchema.SqlServer;

namespace SqlServerConverter.Translate {
	internal class ToSQLiteDatabaseTranslator : InformationSchemaTranslator<SqlServer.Database, SQLite.Database<SqlServer.Database>> {
		private SQLite.Database<SqlServer.Database> currentDatabase = null;
		private SQLite.Catalog<SqlServer.Database> currentCatalog = null;
		private SQLite.Index<SqlServer.Database> currentIndex = null;
		private SQLite.Table<SqlServer.Database> currentTable = null;
		private SQLite.View<SqlServer.Database> currentView = null;

		public ToSQLiteDatabaseTranslator(SqlServer.Database sqlServerDatabase, SQLite.Database<SqlServer.Database> sqliteDatabase) : base(sqlServerDatabase, sqliteDatabase) { }

		public override SQLite.Database<SqlServer.Database> Translate() {
			foreach (SqlServer.Schemata schema in ItemToTranslate.Schemas.Values) {
				//TranslatedItem.Catalogs.Add(schema.CatalogName, new SQLite.Catalog {
				//    CatalogName = schema.CatalogName
				//});

				foreach (SqlServer.Table table in ItemToTranslate.Schemas[schema.SchemaName].Tables.Values) {
					//SQLite.Table<SqlServer.Table> newTable = new SQLite.Table();
					//ToSQLiteTableTranslator newTableTranslator = new ToSQLiteTableTranslator(table, newTable);
				}
			}

			return currentDatabase;
		}
	}
}