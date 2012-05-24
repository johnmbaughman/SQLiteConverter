#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"); to deal in the Software without restriction,
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
using SQLiteConversionEngine.Utility;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;

namespace SQLiteConversionEngine.InformationSchema.SQLite {
	/// <summary>
	/// Description of IndexColumns.
	/// </summary>
	public class IndexColumn : InformationSchemaItemBase<IndexColumn> {

		public IndexColumn(DataRow itemToLoad) : base(itemToLoad) { }

		protected override void LoadFromDataRow() {
			ConstraintCatalog = OriginalItemDataRow["CONSTRAINT_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["CONSTRAINT_CATALOG"].ToString();
			ConstraintSchema = OriginalItemDataRow["CONSTRAINT_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["CONSTRAINT_SCHEMA"].ToString();
			ConstraintName = OriginalItemDataRow["CONSTRAINT_NAME"] == DBNull.Value ? null : OriginalItemDataRow["CONSTRAINT_NAME"].ToString();
			TableCatalog = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
			TableSchema = OriginalItemDataRow["TABLE_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_SCHEMA"].ToString();
			TableName = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
			ColumnName = OriginalItemDataRow["COLUMN_NAME"] == DBNull.Value ? null : OriginalItemDataRow["COLUMN_NAME"].ToString();
			OrdinalPosition = OriginalItemDataRow["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["ORDINAL_POSITION"]);
			IndexName = OriginalItemDataRow["INDEX_NAME"] == DBNull.Value ? null : OriginalItemDataRow["INDEX_NAME"].ToString();
			CollationName = OriginalItemDataRow["COLLATION_NAME"] == DBNull.Value ? null : OriginalItemDataRow["COLLATION_NAME"].ToString();
			SortMode = OriginalItemDataRow["SORT_MODE"] == DBNull.Value ? null : OriginalItemDataRow["SORT_MODE"].ToString();
			ConflictOption = OriginalItemDataRow["CONFLICT_OPTION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["CONFLICT_OPTION"]);
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

		public string ConstraintCatalog { get; set; }

		public string ConstraintSchema { get; set; }

		public string ConstraintName { get; set; }

		public string TableCatalog { get; set; }

		public string TableSchema { get; set; }

		public string TableName { get; set; }

		public string ColumnName { get; set; }

		public int? OrdinalPosition { get; set; }

		public string IndexName { get; set; }

		public string CollationName { get; set; }

		public string SortMode { get; set; }

		public int? ConflictOption { get; set; }
	}
}