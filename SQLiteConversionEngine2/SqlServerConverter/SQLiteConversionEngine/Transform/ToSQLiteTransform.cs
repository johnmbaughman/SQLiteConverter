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

namespace SQLiteConversionEngine.Transform {
	internal class ToSQLiteTransform : ToSQLiteTransformBase<SqlServer.Database> {

		public ToSQLiteTransform(SqlServer.Database sqlServerDatabase) : base(sqlServerDatabase) { }

		public override dynamic Transform() {
			SQLite.Database database = new SQLite.Database();
			foreach (SqlServer.Schemata schemata in ItemToTransform.Schemas.Values) {
				database.Catalogs.Add(schemata.SchemaName, new SQLite.Catalog {
					CatalogName = schemata.SchemaOwner,
				});

				foreach (SqlServer.Table table in schemata.Tables.Values) {
					database.Catalogs[schemata.SchemaName].Tables.Add(table.TableName, new SQLite.Table {
						CatalogName = table.TableCatalog,
						Name = table.TableName
					});

					foreach (SqlServer.Column column in table.Columns.Values) {
						database.Catalogs[schemata.SchemaName].Tables[table.TableName].Columns.Add(column.ColumnName, new SQLite.Column {
							ColumnName = column.ColumnName,
							DataType = column.DataType,
							IsNullable = column.IsNullable,
							NumericPrecision = column.NumericPrecision,
							NumericScale = column.NumericScale,
							OrdinalPosition = column.OrdinalPosition,
							ColumnDefault = column.ColumnDefault
						});
					}

					foreach (SqlServer.Index index in table.Indexes.Values) {
						database.Catalogs[schemata.SchemaName].Tables[table.TableName].Indexes.Add(index.IndexName, new SQLite.Index {
							IndexName = index.IndexName
						});

						foreach (SqlServer.IndexColumn indexColumn in index.IndexColumns.Values) {
							database.Catalogs[schemata.SchemaName].Tables[table.TableName].Indexes[index.IndexName].IndexColumns.Add(indexColumn.ColumnName, new SQLite.IndexColumn {
								ColumnName = indexColumn.ColumnName
							});
						}
					}
				}
			}

			return database;
		}
	}
}