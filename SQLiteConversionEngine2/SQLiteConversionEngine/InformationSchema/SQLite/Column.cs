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
	/// Description of Column.
	/// </summary>
	public class Column<O> : InformationSchemaItemBase<Column<O>, O> {

		public Column(DataRow itemToLoad) : base(itemToLoad) { }

		public string TableCatalog { get; private set; }

		public string TableSchema { get; private set; }

		public string TableName { get; private set; }

		public string ColumnName { get; private set; }

		public Guid? ColumnGuid { get; private set; }

		public long? ColumnPropId { get; private set; }

		public int? OrdinalPosition { get; private set; }

		public bool? ColumnHasDefault { get; private set; }

		public string ColumnDefault { get; private set; }

		public long? ColumnFlags { get; private set; }

		public bool? IsNullable { get; private set; }

		public string DataType { get; private set; }

		public Guid? TypeGuid { get; private set; }

		public int? CharacterMaximumLength { get; private set; }

		public int? CharacterOctetLength { get; private set; }

		public int? NumericPrecision { get; private set; }

		public int? NumericScale { get; private set; }

		public long? DateTimePrecision { get; private set; }

		public string CharacterSetCatalog { get; private set; }

		public string CharacterSetSchema { get; private set; }

		public string CharacterSetName { get; private set; }

		public string CollationCatalog { get; private set; }

		public string CollationSchema { get; private set; }

		public string CollationName { get; private set; }

		public string DomainCatalog { get; private set; }

		public string DomainName { get; private set; }

		public string Description { get; private set; }

		public bool? PrimaryKey { get; private set; }

		public string EdmType { get; private set; }

		public bool? AutoIncrement { get; private set; }

		public bool? Unique { get; private set; }
	}
}