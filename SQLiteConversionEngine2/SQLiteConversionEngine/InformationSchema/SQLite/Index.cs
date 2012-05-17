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

namespace SQLiteConversionEngine.InformationSchema.SQLite {
	/// <summary>
	/// Description of Index.
	/// </summary>
	public class Index : InformationSchemaItemBase {

		public Index() {
			IndexColumns = new IndexColumnCollection();
		}

		public string TableCatalog { get; set; }

		public string TableSchema { get; set; }

		public string TableName { get; set; }

		public string IndexCatalog { get; set; }

		public string IndexSchema { get; set; }

		public string IndexName { get; set; }

		public bool? PrimaryKey { get; set; }

		public bool? Unique { get; set; }

		public bool? Clustered { get; set; }

		public int? Type { get; set; }

		public int? FillFactor { get; set; }

		public int? InitialSize { get; set; }

		public int? Nulls { get; set; }

		public bool? SortBookmarks { get; set; }

		public bool? AutoUpdate { get; set; }

		public int? NullCollation { get; set; }

		public int? OrdinalPosition { get; set; }

		public string ColumnName { get; set; }

		public Guid? ColumnGUID { get; set; }

		public long? ColumnPropId { get; set; }

		public short? Collation { get; set; }

		public decimal? Cardinality { get; set; }

		public int? Pages { get; set; }

		public string FilterCondition { get; set; }

		public bool? Integrated { get; set; }

		public string IndexDefinition { get; set; }

		public IndexColumnCollection IndexColumns { get; set; }
	}
}