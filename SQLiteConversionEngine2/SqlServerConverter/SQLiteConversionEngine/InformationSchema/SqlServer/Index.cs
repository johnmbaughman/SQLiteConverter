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
	public class Index : InformationSchemaItemBase<Index> {

		public Index(DataRow itemToLoad)
			: base(itemToLoad) {
			IndexColumns = new IndexColumnCollection();
		}

		protected override void LoadFromDataRow() {
			ConstraintCatalog = OriginalItemDataRow["constraint_catalog"] == DBNull.Value ? null : OriginalItemDataRow["constraint_catalog"].ToString();
			ConstraintSchema = OriginalItemDataRow["constraint_schema"] == DBNull.Value ? null : OriginalItemDataRow["constraint_schema"].ToString();
			ConstraintName = OriginalItemDataRow["constraint_name"] == DBNull.Value ? null : OriginalItemDataRow["constraint_name"].ToString();
			TableCatalog = OriginalItemDataRow["table_catalog"] == DBNull.Value ? null : OriginalItemDataRow["table_catalog"].ToString();
			TableSchema = OriginalItemDataRow["table_schema"] == DBNull.Value ? null : OriginalItemDataRow["table_schema"].ToString();
			TableName = OriginalItemDataRow["table_name"] == DBNull.Value ? null : OriginalItemDataRow["table_name"].ToString();
			IndexName = OriginalItemDataRow["index_name"] == DBNull.Value ? null : OriginalItemDataRow["index_name"].ToString();
			TypeDescription = OriginalItemDataRow["type_desc"] == DBNull.Value ? null : OriginalItemDataRow["type_desc"].ToString();
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

		public IndexColumnCollection IndexColumns { get; internal set; }

		public string ConstraintCatalog { get; internal set; }

		public string ConstraintSchema { get; internal set; }

		public string ConstraintName { get; internal set; }

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string IndexName { get; internal set; }

		public string TypeDescription { get; internal set; }
	}
}