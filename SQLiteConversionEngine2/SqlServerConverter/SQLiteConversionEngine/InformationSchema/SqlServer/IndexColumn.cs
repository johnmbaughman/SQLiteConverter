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
using System.Data;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;

namespace SQLiteConversionEngine.InformationSchema.SqlServer {
	public class IndexColumn : InformationSchemaItemBase<IndexColumn, SQLite.IndexCollection<IndexColumn>> {

		public IndexColumn(DataRow itemToLoad)
			: base(itemToLoad) {
			ConstraintCatalog = itemToLoad["constraint_catalog"] == DBNull.Value ? null : itemToLoad["constraint_catalog"].ToString();
			ConstraintSchema = itemToLoad["constraint_schema"] == DBNull.Value ? null : itemToLoad["constraint_schema"].ToString();
			ConstraintName = itemToLoad["constraint_name"] == DBNull.Value ? null : itemToLoad["constraint_name"].ToString();
			TableCatalog = itemToLoad["table_catalog"] == DBNull.Value ? null : itemToLoad["table_catalog"].ToString();
			TableSchema = itemToLoad["table_schema"] == DBNull.Value ? null : itemToLoad["table_schema"].ToString();
			TableName = itemToLoad["table_name"] == DBNull.Value ? null : itemToLoad["table_name"].ToString();
			ColumnName = itemToLoad["column_name"] == DBNull.Value ? null : itemToLoad["column_name"].ToString();
			OrdinalPosition = itemToLoad["ordinal_position"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["ordinal_position"]);
			KeyType = itemToLoad["KeyType"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["KeyType"]);
			IndexName = itemToLoad["index_name"] == DBNull.Value ? null : itemToLoad["index_name"].ToString();
		}

		public string ConstraintCatalog { get; internal set; }

		public string ConstraintSchema { get; internal set; }

		public string ConstraintName { get; internal set; }

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string ColumnName { get; internal set; }

		public int? OrdinalPosition { get; internal set; }

		public int? KeyType { get; internal set; }

		public string IndexName { get; internal set; }
	}
}