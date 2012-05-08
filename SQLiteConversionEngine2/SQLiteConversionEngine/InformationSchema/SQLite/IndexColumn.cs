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
	/// Description of IndexColumns.
	/// </summary>
	public class IndexColumn {

		public IndexColumn() {
		}

		public string ConstraintCatalog { get; internal set; }

		public string ConstraintSchema { get; internal set; }

		public string ConstraintName { get; internal set; }

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string ColumnName { get; internal set; }

		public int? OrdinalPosition { get; internal set; }

		public string IndexName { get; internal set; }

		public string CollationName { get; internal set; }

		public string SortMode { get; internal set; }

		public int? ConflictOption { get; internal set; }

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