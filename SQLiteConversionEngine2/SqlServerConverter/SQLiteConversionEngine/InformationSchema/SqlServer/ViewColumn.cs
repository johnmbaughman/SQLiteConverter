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
using SQLiteConversionEngine.Utility;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;

namespace SQLiteConversionEngine.InformationSchema.SqlServer {
	public class ViewColumn : InformationSchemaItemBase<ViewColumn> {

		public ViewColumn(DataRow itemToLoad) : base(itemToLoad) { }

		protected override void LoadFromDataRow() {
			ViewCatalog = OriginalItemDataRow["VIEW_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["VIEW_CATALOG"].ToString();
			ViewSchema = OriginalItemDataRow["VIEW_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["VIEW_SCHEMA"].ToString();
			ViewName = OriginalItemDataRow["VIEW_NAME"] == DBNull.Value ? null : OriginalItemDataRow["VIEW_NAME"].ToString();
			TableCatalog = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
			TableSchema = OriginalItemDataRow["TABLE_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_SCHEMA"].ToString();
			TableName = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
			ColumnName = OriginalItemDataRow["COLUMN_NAME"] == DBNull.Value ? null : OriginalItemDataRow["COLUMN_NAME"].ToString();
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

		public string ViewCatalog { get; internal set; }

		public string ViewSchema { get; internal set; }

		public string ViewName { get; internal set; }

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string ColumnName { get; internal set; }
	}
}