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

namespace SQLiteConversionEngine.InformationSchema.SqlServer {
	public class Schemata : InformationSchemaItemBase<Schemata> {

		public Schemata(DataRow itemToLoad)
			: base(itemToLoad) {
			Tables = new TableCollection();
			Views = new ViewCollection();
		}

		protected override void LoadFromDataRow() {
			CatalogName = OriginalItemDataRow["CATALOG_NAME"] == DBNull.Value ? null : OriginalItemDataRow["CATALOG_NAME"].ToString();
			SchemaName = OriginalItemDataRow["SCHEMA_NAME"] == DBNull.Value ? null : OriginalItemDataRow["SCHEMA_NAME"].ToString();
			SchemaOwner = OriginalItemDataRow["SCHEMA_OWNER"] == DBNull.Value ? null : OriginalItemDataRow["SCHEMA_OWNER"].ToString();
			DefaultCharacterSetCatalog = OriginalItemDataRow["DEFAULT_CHARACTER_SET_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["DEFAULT_CHARACTER_SET_CATALOG"].ToString();
			DefaultCharacterSetSchema = OriginalItemDataRow["DEFAULT_CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["DEFAULT_CHARACTER_SET_SCHEMA"].ToString();
			DefaultCharacterSetName = OriginalItemDataRow["DEFAULT_CHARACTER_SET_NAME"] == DBNull.Value ? null : OriginalItemDataRow["DEFAULT_CHARACTER_SET_NAME"].ToString();
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

		public TableCollection Tables { get; internal set; }

		public ViewCollection Views { get; internal set; }

		public string CatalogName { get; internal set; }

		public string SchemaName { get; internal set; }

		public string SchemaOwner { get; internal set; }

		public string DefaultCharacterSetCatalog { get; internal set; }

		public string DefaultCharacterSetSchema { get; internal set; }

		public string DefaultCharacterSetName { get; internal set; }
	}
}