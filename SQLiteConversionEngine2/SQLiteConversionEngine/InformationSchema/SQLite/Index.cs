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
using System.Reflection;
using System.Text;

namespace SQLiteConversionEngine.InformationSchema.SQLite {
	/// <summary>
	/// Description of Index.
	/// </summary>
	public class Index {

		public Index() {
			IndexColumns = new IndexColumnCollection();
		}

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string IndexCatalog { get; internal set; }

		public string IndexSchema { get; internal set; }

		public string IndexName { get; internal set; }

		public bool? PrimaryKey { get; internal set; }

		public bool? Unique { get; internal set; }

		public bool? Clustered { get; internal set; }

		public int? Type { get; internal set; }

		public int? FillFactor { get; internal set; }

		public int? InitialSize { get; internal set; }

		public int? Nulls { get; internal set; }

		public bool? SortBookmarks { get; internal set; }

		public bool? AutoUpdate { get; internal set; }

		public int? NullCollation { get; internal set; }

		public int? OrdinalPosition { get; internal set; }

		public string ColumnName { get; internal set; }

		public Guid? ColumnGUID { get; internal set; }

		public long? ColumnPropId { get; internal set; }

		public short? Collation { get; internal set; }

		public decimal? Cardinality { get; internal set; }

		public int? Pages { get; internal set; }

		public string FilterCondition { get; internal set; }

		public bool? Integrated { get; internal set; }

		public string IndexDefinition { get; internal set; }

		public IndexColumnCollection IndexColumns { get; internal set; }

		public override string ToString() {
			StringBuilder sc = new StringBuilder();

			foreach (PropertyInfo propertyItem in this.GetType().GetProperties()) {
				string propName = propertyItem.Name.ToString();
				var tempVal = propertyItem.GetValue(this, null);
				var propVal = tempVal == null ? string.Empty : tempVal;

				sc.AppendFormat("{0} : {1}{2}", propName, propVal, Environment.NewLine);
			}

			return sc.ToString();
		}
	}
}