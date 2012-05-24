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

namespace SQLiteConversionEngine.InformationSchema.SQLite {
	/// <summary>
	/// Description of Table.
	/// </summary>
	public class Table : InformationSchemaItemBase<Table> {

		public Table(DataRow itemToLoad)
			: base(itemToLoad) {
			Columns = new ColumnCollection();
			ForeignKeys = new ForeignKeyCollection();
			Indexes = new IndexCollection();
			//Triggers = new TriggerCollection<O>();
		}

		protected override void LoadFromDataRow() {
			CatalogName = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
			Name = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
			Type = OriginalItemDataRow["TABLE_TYPE"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_TYPE"].ToString();
			Id = OriginalItemDataRow["TABLE_ID"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(OriginalItemDataRow["TABLE_ID"]);
			RootPage = OriginalItemDataRow["TABLE_ROOTPAGE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["TABLE_ROOTPAGE"]);
			Definition = OriginalItemDataRow["TABLE_DEFINITION"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_DEFINITION"].ToString();
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

		public string CatalogName { get; private set; }

		public string Name { get; private set; }

		public string Type { get; private set; }

		public long? Id { get; private set; }

		public int? RootPage { get; private set; }

		public string Definition { get; private set; }

		public ColumnCollection Columns { get; private set; }

		public ForeignKeyCollection ForeignKeys { get; private set; }

		public IndexCollection Indexes { get; private set; }

		//public TriggerCollection<O> Triggers { get; private set; }
	}
}