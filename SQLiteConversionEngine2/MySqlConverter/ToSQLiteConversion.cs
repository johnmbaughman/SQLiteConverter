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
using SQLiteConversionEngine.InformationSchema.MySql;
using SQLiteConversionEngine.Utility;

namespace MySqlConverter
{
    internal class ToSQLiteConversion : ToSQLiteConversionBase<Database>
    {
        private SqlConnection sqlConnection = null;
        private string currentSchemaName = string.Empty;
        private string currentViewName = string.Empty;
        private string currentTableName = string.Empty;
        private string currentIndexName = string.Empty;
        private List<string> defaultIgnores = new List<string> { "sysdiagrams" };

        public ToSQLiteConversion(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) : base(sqliteConnectionStringSettings, otherConnectionStringSettings) { }

        public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers)
        {
            using (sqlConnection = new SqlConnection(Connections.OtherConnection.ConnectionString))
            {
                try
                {
                    sqlConnection.Open();
                    LoadSchema();
                    sqlConnection.Close();
                }
                catch (Exception)
                {
                    if (sqlConnection.State == ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                    throw;
                }
            }
        }

        protected override void ConvertSourceDatabaseToDestination(ConversionBase<Database>.ConversionHandler conversionHandler, ConversionBase<Database>.TableSelectionHandler tableSelectionHandler, ConversionBase<Database>.FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers)
        {
            throw new NotImplementedException();
        }

        protected override void CopySourceDataToDestination(ConversionBase<Database>.ConversionHandler conversionHandler)
        {
            throw new NotImplementedException();
        }

        protected override void LoadColumns()
        {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Columns, new string[] { null, currentSchemaName, currentTableName, null }))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Columns.Add(row["COLUMN_NAME"].ToString(), new Column
                    {
                        TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
                        TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
                        ColumnName = row["COLUMN_NAME"] == DBNull.Value ? null : row["COLUMN_NAME"].ToString(),
                        OrdinalPosition = row["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["ORDINAL_POSITION"]),
                        ColumnDefault = row["COLUMN_DEFAULT"] == DBNull.Value ? null : row["COLUMN_DEFAULT"].ToString(),
                        IsNullable = row["IS_NULLABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_NULLABLE"].ToString()),
                        DataType = row["DATA_TYPE"] == DBNull.Value ? null : row["DATA_TYPE"].ToString(),
                        CharacterMaximumLength = row["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]),
                        CharacterOctetLength = row["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["CHARACTER_OCTET_LENGTH"]),
                        NumericPrecision = row["NUMERIC_PRECISION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NUMERIC_PRECISION"]),
                        NumericPrecisionRadix = row["NUMERIC_PRECISION_RADIX"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NUMERIC_PRECISION_RADIX"]),
                        NumericScale = row["NUMERIC_SCALE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["NUMERIC_SCALE"]),
                        DateTimePrecision = row["DATETIME_PRECISION"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt32(row["DATETIME_PRECISION"]),
                        CharacterSetCatalog = row["CHARACTER_SET_CATALOG"] == DBNull.Value ? null : row["CHARACTER_SET_CATALOG"].ToString(),
                        CharacterSetSchema = row["CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : row["CHARACTER_SET_SCHEMA"].ToString(),
                        CharacterSetName = row["CHARACTER_SET_NAME"] == DBNull.Value ? null : row["CHARACTER_SET_NAME"].ToString(),
                        CollationCatalog = row["COLLATION_CATALOG"] == DBNull.Value ? null : row["COLLATION_CATALOG"].ToString(),
                        IsSparse = row["IS_SPARSE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_SPARSE"].ToString()),
                        IsColumnSet = row["IS_COLUMN_SET"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_COLUMN_SET"].ToString()),
                        IsFileStream = row["IS_FILESTREAM"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_FILESTREAM"].ToString()),
                    });
                }
            }
        }

        protected override void LoadForeignKeys()
        {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.ForeignKeys, new string[] { null, currentSchemaName, currentTableName, null }))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].ForeignKeys.Add(row["CONSTRAINT_NAME"].ToString(), new ForeignKey
                    {
                        ConstraintCatalog = row["CONSTRAINT_CATALOG"] == DBNull.Value ? null : row["CONSTRAINT_CATALOG"].ToString(),
                        ConstraintSchema = row["CONSTRAINT_SCHEMA"] == DBNull.Value ? null : row["CONSTRAINT_SCHEMA"].ToString(),
                        ConstraintName = row["CONSTRAINT_NAME"] == DBNull.Value ? null : row["CONSTRAINT_NAME"].ToString(),
                        TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
                        TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
                        ConstraintType = row["CONSTRAINT_TYPE"] == DBNull.Value ? null : row["CONSTRAINT_TYPE"].ToString(),
                        IsDeferrable = row["IS_DEFERRABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_DEFERRABLE"].ToString()),
                        InitiallyDeferred = row["INITIALLY_DEFERRED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["INITIALLY_DEFERRED"].ToString())
                    });
                }
            }
        }

        protected override void LoadIndexColumns()
        {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, new string[] { null, currentSchemaName, currentTableName }))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes[currentIndexName].IndexColumns.Add(row["column_name"].ToString(), new IndexColumn
                    {
                        ConstraintCatalog = row["constraint_catalog"] == DBNull.Value ? null : row["constraint_catalog"].ToString(),
                        ConstraintSchema = row["constraint_schema"] == DBNull.Value ? null : row["constraint_schema"].ToString(),
                        ConstraintName = row["constraint_name"] == DBNull.Value ? null : row["constraint_name"].ToString(),
                        TableCatalog = row["table_catalog"] == DBNull.Value ? null : row["table_catalog"].ToString(),
                        TableSchema = row["table_schema"] == DBNull.Value ? null : row["table_schema"].ToString(),
                        TableName = row["table_name"] == DBNull.Value ? null : row["table_name"].ToString(),
                        ColumnName = row["column_name"] == DBNull.Value ? null : row["column_name"].ToString(),
                        OrdinalPosition = row["ordinal_position"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["ordinal_position"]),
                        KeyType = row["KeyType"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(row["KeyType"]),
                        IndexName = row["index_name"] == DBNull.Value ? null : row["index_name"].ToString()
                    });
                }
            }
        }

        protected override void LoadIndexes()
        {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Indexes, new string[] { null, currentSchemaName, currentTableName }))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes.Add(row["index_name"].ToString(), new Index
                    {
                        ConstraintCatalog = row["constraint_catalog"] == DBNull.Value ? null : row["constraint_catalog"].ToString(),
                        ConstraintSchema = row["constraint_schema"] == DBNull.Value ? null : row["constraint_schema"].ToString(),
                        ConstraintName = row["constraint_name"] == DBNull.Value ? null : row["constraint_name"].ToString(),
                        TableCatalog = row["table_catalog"] == DBNull.Value ? null : row["table_catalog"].ToString(),
                        TableSchema = row["table_schema"] == DBNull.Value ? null : row["table_schema"].ToString(),
                        TableName = row["table_name"] == DBNull.Value ? null : row["table_name"].ToString(),
                        IndexName = row["index_name"] == DBNull.Value ? null : row["index_name"].ToString(),
                        TypeDescription = row["type_desc"] == DBNull.Value ? null : row["type_desc"].ToString()
                    });
                }
            }

            foreach (Index catalog in SourceSchema.Schemas[currentSchemaName].Tables[currentTableName].Indexes.Values)
            {
                currentIndexName = catalog.IndexName;
                LoadIndexColumns();
            }
        }

        protected override void LoadSchema()
        {
            string sql = "SELECT [CATALOG_NAME] ,[SCHEMA_NAME] ,[SCHEMA_OWNER] ,[DEFAULT_CHARACTER_SET_CATALOG] ,[DEFAULT_CHARACTER_SET_SCHEMA] ,[DEFAULT_CHARACTER_SET_NAME] FROM [INFORMATION_SCHEMA].[SCHEMATA]{0}";
            string where = string.Format(" {0}{1}", SchemasToLoad.Count > 0 ? "where" : string.Empty, Utilities.ConvertListToInClause(SchemasToLoad, "SCHEMA_NAME"));

            using (SqlCommand sqlCommand = new SqlCommand(string.Format(sql, where), sqlConnection))
            {
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SourceSchema.Schemas.Add(reader["SCHEMA_NAME"].ToString(), new Schemata
                        {
                            CatalogName = reader["CATALOG_NAME"] == DBNull.Value ? null : reader["CATALOG_NAME"].ToString(),
                            SchemaName = reader["SCHEMA_NAME"] == DBNull.Value ? null : reader["SCHEMA_NAME"].ToString(),
                            SchemaOwner = reader["SCHEMA_OWNER"] == DBNull.Value ? null : reader["SCHEMA_OWNER"].ToString(),
                            DefaultCharacterSetCatalog = reader["DEFAULT_CHARACTER_SET_CATALOG"] == DBNull.Value ? null : reader["DEFAULT_CHARACTER_SET_CATALOG"].ToString(),
                            DefaultCharacterSetSchema = reader["DEFAULT_CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : reader["DEFAULT_CHARACTER_SET_SCHEMA"].ToString(),
                            DefaultCharacterSetName = reader["DEFAULT_CHARACTER_SET_NAME"] == DBNull.Value ? null : reader["DEFAULT_CHARACTER_SET_NAME"].ToString(),
                        });
                    }
                }
            }

            foreach (Schemata catalog in SourceSchema.Schemas.Values)
            {
                currentSchemaName = catalog.SchemaName;
                LoadTables();
                LoadViews();
            }
        }

        protected override void LoadTables()
        {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Tables, new string[] { null, currentSchemaName, null, "BASE TABLE" }))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    if (!defaultIgnores.Contains(row["TABLE_NAME"].ToString()))
                    {
                        Schemata schema = SourceSchema.Schemas[currentSchemaName];
                        schema.Tables.Add(row["TABLE_NAME"].ToString(), new Table()
                        {
                            TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
                            TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
                            TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
                            TableType = row["TABLE_TYPE"] == DBNull.Value ? null : row["TABLE_TYPE"].ToString()
                        });
                    }
                }
            }

            foreach (Table table in SourceSchema.Schemas[currentSchemaName].Tables.Values)
            {
                currentTableName = table.TableName;
                LoadColumns();
                LoadForeignKeys();
                LoadIndexes();
                //LoadTriggers();
            }
        }

        protected override void LoadTriggers()
        {
            throw new NotImplementedException();
        }

        protected override void LoadViewColumns()
        {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.ViewColumns, new string[] { null, currentSchemaName, currentViewName, null }))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    SourceSchema.Schemas[currentSchemaName].Views[currentViewName].ViewColumns.Add(row["COLUMN_NAME"].ToString(), new ViewColumn
                    {
                        ViewCatalog = row["VIEW_CATALOG"] == DBNull.Value ? null : row["VIEW_CATALOG"].ToString(),
                        ViewSchema = row["VIEW_SCHEMA"] == DBNull.Value ? null : row["VIEW_SCHEMA"].ToString(),
                        ViewName = row["VIEW_NAME"] == DBNull.Value ? null : row["VIEW_NAME"].ToString(),
                        TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
                        TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
                        ColumnName = row["COLUMN_NAME"] == DBNull.Value ? null : row["COLUMN_NAME"].ToString()
                    });
                }
            }
        }

        protected override void LoadViews()
        {
            using (DataTable dataTable = sqlConnection.GetSchema(SqlClientMetaDataCollectionNames.Views, new string[] { null, currentSchemaName, null }))
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    SourceSchema.Schemas[currentSchemaName].Views.Add(row["TABLE_NAME"].ToString(), new View
                    {
                        TableCatalog = row["TABLE_CATALOG"] == DBNull.Value ? null : row["TABLE_CATALOG"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"] == DBNull.Value ? null : row["TABLE_SCHEMA"].ToString(),
                        TableName = row["TABLE_NAME"] == DBNull.Value ? null : row["TABLE_NAME"].ToString(),
                        CheckOption = row["CHECK_OPTION"] == DBNull.Value ? null : row["CHECK_OPTION"].ToString(),
                        IsUpdatable = row["IS_UPDATABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(row["IS_UPDATABLE"].ToString())
                    });

                    foreach (View catalog in SourceSchema.Schemas[currentSchemaName].Views.Values)
                    {
                        currentViewName = catalog.TableName;
                        LoadViewColumns();
                    }
                }
            }
        }
    }
}