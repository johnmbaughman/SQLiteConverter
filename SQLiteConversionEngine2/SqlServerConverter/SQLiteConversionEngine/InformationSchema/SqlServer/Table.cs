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
	public class Table : InformationSchemaItemBase<Table, SQLite.Table<Table>> {

		public Table(DataRow itemToLoad)
			: base(itemToLoad) {
			Columns = new ColumnCollection();
			ForeignKeys = new ForeignKeyCollection();
			Indexes = new IndexCollection();
			Triggers = new TriggerCollection();

			TableCatalog = itemToLoad["TABLE_CATALOG"] == DBNull.Value ? null : itemToLoad["TABLE_CATALOG"].ToString();
			TableName = itemToLoad["TABLE_NAME"] == DBNull.Value ? null : itemToLoad["TABLE_NAME"].ToString();
			TableSchema = itemToLoad["TABLE_SCHEMA"] == DBNull.Value ? null : itemToLoad["TABLE_SCHEMA"].ToString();
			TableType = itemToLoad["TABLE_TYPE"] == DBNull.Value ? null : itemToLoad["TABLE_TYPE"].ToString();
		}

		public Table(SQLite.Table<Table> itemToLoad)
			: base(itemToLoad) {
		}

		public ColumnCollection Columns { get; internal set; }

		public ForeignKeyCollection ForeignKeys { get; internal set; }

		public IndexCollection Indexes { get; internal set; }

		public TriggerCollection Triggers { get; internal set; }

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string TableType { get; internal set; }
	}
}