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

using SQLiteConversionEngine.InformationSchema;
using MySql = SQLiteConversionEngine.InformationSchema.MySql;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;

namespace MySqlConverter
{
    internal class FromSQLiteTranslator : InformationSchemaTranslator<SQLite.Database, MySql.Database>
    {
        private MySql.Schemata currentSchema = null;
        private const string defaultSchemaName = "dbo";

        public FromSQLiteTranslator(SQLite.Database sqliteDatabase) : base(sqliteDatabase) { }

        public override MySql.Database Translate()
        {
            MySql.Database database = new MySql.Database();

            foreach (SQLite.Catalog catalog in DatabaseToTranslate.Catalogs.Values)
            {
                currentSchema = new MySql.Schemata
                {
                    CatalogName = catalog.CatalogName,
                    SchemaName = defaultSchemaName,
                    SchemaOwner = defaultSchemaName
                };

                foreach (SQLite.Table table in catalog.Tables.Values)
                {
                    currentSchema.Tables.Add(table.Name, TranslateTable(table));
                }

                foreach (SQLite.View view in catalog.Views.Values)
                {
                    currentSchema.Views.Add(view.TableName, TranslateView(view));
                }

                database.Schemas.Add(defaultSchemaName, currentSchema);
            }

            return database;
        }

        private MySql.Table TranslateTable(SQLite.Table inTable)
        {
            MySql.Table table = new MySql.Table
            {
                TableCatalog = inTable.CatalogName,
                TableName = inTable.Name,
                TableSchema = defaultSchemaName,
                TableType = inTable.Type
            };

            foreach (SQLite.Column column in inTable.Columns.Values)
            {
                table.Columns.Add(column.ColumnName, TranslateColumn(column));
            }

            foreach (SQLite.Index index in inTable.Indexes.Values)
            {
                table.Indexes.Add(index.IndexName, TranslateIndex(index));
            }

            foreach (SQLite.ForeignKey foreignKey in inTable.ForeignKeys.Values)
            {
                table.ForeignKeys.Add(foreignKey.ConstraintName, TranslateForeignKey(foreignKey));
            }
            return table;
        }

        private MySql.Column TranslateColumn(SQLite.Column inColumn)
        {
            return new MySql.Column
            {
                CharacterMaximumLength = inColumn.CharacterMaximumLength,
                CharacterOctetLength = inColumn.CharacterOctetLength,
                CharacterSetCatalog = inColumn.CharacterSetCatalog,
                CharacterSetName = inColumn.CharacterSetName,
                CharacterSetSchema = inColumn.CharacterSetSchema,
                CollationCatalog = inColumn.CollationCatalog,
                ColumnDefault = inColumn.ColumnDefault,
                ColumnName = inColumn.ColumnName,
                DataType = inColumn.DataType,
                DateTimePrecision = inColumn.DateTimePrecision,
                IsNullable = inColumn.IsNullable,
                NumericPrecision = inColumn.NumericPrecision,
                NumericScale = inColumn.NumericScale,
                OrdinalPosition = inColumn.OrdinalPosition,
                TableCatalog = inColumn.TableCatalog,
                TableName = inColumn.TableCatalog,
                TableSchema = inColumn.TableSchema
            };
        }

        private MySql.Index TranslateIndex(SQLite.Index inIndex)
        {
            MySql.Index index = new MySql.Index
            {
            };

            foreach (SQLite.IndexColumn indexColumn in inIndex.IndexColumns.Values)
            {
                index.IndexColumns.Add(indexColumn.ColumnName, TranslateIndexColumn(indexColumn));
            }

            return index;
        }

        private MySql.IndexColumn TranslateIndexColumn(SQLite.IndexColumn inIndexColumn)
        {
            return new MySql.IndexColumn
            {
            };
        }

        private MySql.ForeignKey TranslateForeignKey(SQLite.ForeignKey inForeignKey)
        {
            return new MySql.ForeignKey
            {
            };
        }

        private MySql.View TranslateView(SQLite.View inView)
        {
            MySql.View view = new MySql.View
            {
            };

            foreach (SQLite.ViewColumn viewColumn in inView.ViewColumns.Values)
            {
                view.ViewColumns.Add(viewColumn.ColumnName, TranslateViewColumn(viewColumn));
            }

            return view;
        }

        private MySql.ViewColumn TranslateViewColumn(SQLite.ViewColumn inViewColumn)
        {
            return new MySql.ViewColumn
            {
            };
        }
    }
}