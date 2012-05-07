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
using Massive.SQLite;
using SQLiteConversionEngine.InformationSchema;

namespace SQLiteConversionEngine.Conversion {
	public abstract class FromSQLiteConversionBase : ConversionBase {

		public FromSQLiteConversionBase(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings)
			: base(sqliteConnectionStringSettings, otherConnectionStringSettings) {
			string[] connectionString = sqliteConnectionStringSettings.ConnectionString.Split(';');
			foreach (string param in connectionString) {
				if (param.ToLower().Contains("data source")) {
					SourceSchema.Name = param.Split('=')[1];
					SourceSchema.Schema = string.Empty;
					SourceSchema.Database.Name = SourceSchema.Name;
					SourceSchema.Database.Schema = SourceSchema.Schema;
				}
			}
		}

		protected override void ConvertSourceDatabaseToDestination(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			throw new NotImplementedException();
		}

		public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			ReadSourceSchema(null, null);
		}

		protected override void CopySourceDataToDestination(ConversionHandler conversionHandler) {
			throw new NotImplementedException();
		}

		protected override void CreateForeignKeySchema(Table table) {
			throw new NotImplementedException();
		}

		protected override Table CreateTableSchema(string tableName, string schemaName) {
			throw new NotImplementedException();
		}

		protected override void ReadSourceSchema(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler) {
			// Read table info
			var sqliteMaster = new DynamicModel(connections.SQLiteConnection, "sqlite_master", "name");
			var tables = sqliteMaster.All(where: "type='table' and tbl_name <> 'sqlite_sequence'");

			// Loop tables
			foreach (var table in tables) {
				// Add table info to schema
				Table sourceTable = new Table {
					Name = table.tbl_name,
					Schema = SourceSchema.Schema
				};

				// Read column info
				var columns = Massive.SQLite.DynamicModel.Open(connections.SQLiteConnection).Query(string.Format("pragma table_info('{0}')", table.tbl_name));
				foreach (var column in columns) {
					sourceTable.Columns.Add(new Column {
						Name = column.name,
						ColumnType = column.type,
						IsPrimaryKey = column.pk == 1 ? true : false,
						Schema = SourceSchema.Schema + "." + sourceTable.Name
					});
				}

				// Read foreign key info
				var fkeys = Massive.SQLite.DynamicModel.Open(connections.SQLiteConnection).Query(string.Format("pragma foreign_key_list('{0}')", table.tbl_name));
				foreach (var fkey in fkeys) {
					sourceTable.ForeignKeys.Add(new ForeignKey {
						ForeignKeyTable = fkey.table,
						FromColumn = fkey.from,
						Match = fkey.match,
						OnDelete = fkey.on_delete,
						OnUpdate = fkey.on_update,
						ToColumn = fkey.to,
						Schema = SourceSchema.Schema + "." + sourceTable.Name
					});
				}

				SourceSchema.Tables.Add(sourceTable);
			}
		}
	}
}