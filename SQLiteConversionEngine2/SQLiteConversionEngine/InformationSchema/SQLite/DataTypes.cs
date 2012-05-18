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
	/// Description of DataType.
	/// </summary>
	public class DataTypes<O> : InformationSchemaItemBase<DataTypes<O>, O> {

		public DataTypes(DataRow itemToLoad) : base(itemToLoad) { }

		public string TypeName { get; set; }

		public int? ProviderDbType { get; set; }

		public long? ColumnSize { get; set; }

		public string CreateFormat { get; set; }

		public string CreateParameters { get; set; }

		public string DataType { get; set; }

		public bool? IsAutoIncrementable { get; set; }

		public bool? IsBestMatch { get; set; }

		public bool? IsCaseSensitive { get; set; }

		public bool? IsFixedLength { get; set; }

		public bool? IsFixedPrecisionScale { get; set; }

		public bool? IsLong { get; set; }

		public bool? IsNullable { get; set; }

		public bool? IsSearchable { get; set; }

		public bool? IsSearchableWithLike { get; set; }

		public bool? IsLiteralSupported { get; set; }

		public string LiteralPrefix { get; set; }

		public string LiteralSuffix { get; set; }

		public bool? IsUnsigned { get; set; }

		public short? MaximumScale { get; set; }

		public short? MinimumScale { get; set; }

		public bool? IsConcurrencyType { get; set; }
	}
}