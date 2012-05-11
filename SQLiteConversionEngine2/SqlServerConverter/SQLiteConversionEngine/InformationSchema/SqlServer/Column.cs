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

namespace SQLiteConversionEngine.InformationSchema.SqlServer {
	public class Column : InformationSchemaItemBase {

		public Column() { }

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string ColumnName { get; internal set; }

		public int? OrdinalPosition { get; internal set; }

		public string ColumnDefault { get; internal set; }

		public bool? IsNullable { get; internal set; }

		public string DataType { get; internal set; }

		public int? CharacterMaximumLength { get; internal set; }

		public int? CharacterOctetLength { get; internal set; }

		public int? NumericPrecision { get; internal set; }

		public int? NumericPrecisionRadix { get; internal set; }

		public int? NumericScale { get; internal set; }

		public long? DateTimePrecision { get; internal set; }

		public string CharacterSetCatalog { get; internal set; }

		public string CharacterSetSchema { get; internal set; }

		public string CharacterSetName { get; internal set; }

		public string CollationCatalog { get; internal set; }

		public bool? IsSparse { get; internal set; }

		public bool? IsColumnSet { get; internal set; }

		public bool? IsFileStream { get; internal set; }
	}
}