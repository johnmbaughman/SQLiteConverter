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

namespace SQLiteConversionEngine.InformationSchema {
	public class ForeignKey : InformationSchemaBase {

		/// <summary>
		/// Gets or sets the foreign key table.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The foreign key table.</value>
		public string ForeignKeyTable { get; set; }

		/// <summary>
		/// Gets or sets From (local) column.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>From column.</value>
		public string FromColumn { get; set; }

		/// <summary>
		/// Gets or sets To (foreign) column.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>To column.</value>
		public string ToColumn { get; set; }

		/// <summary>
		/// Gets or sets the On Update action.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The on update.</value>
		public string OnUpdate { get; set; }

		/// <summary>
		/// Gets or sets the On Delete action.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The on delete.</value>
		public string OnDelete { get; set; }

		/// <summary>
		/// Gets or sets the Match action.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The match.</value>
		public string Match { get; set; }
	}
}