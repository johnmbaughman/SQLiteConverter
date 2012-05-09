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
	public class Index : InformationSchemaItemBase {

		public Index() {
			IndexColumns = new IndexColumnCollection();
		}

		public IndexColumnCollection IndexColumns { get; internal set; }

		public int ObjectId { get; internal set; }

		public string Name { get; internal set; }

		public int? IndexId { get; internal set; }

		public int? Type { get; internal set; }

		public string TypeDescription { get; internal set; }

		public bool? IsUnique { get; internal set; }

		public int? DataSpaceId { get; internal set; }

		public bool? IgnoreDupKey { get; internal set; }

		public bool? IsPrimaryKey { get; internal set; }

		public bool? IsUniqueConstraint { get; internal set; }

		public int? FillFactor { get; internal set; }

		public bool? IsPadded { get; internal set; }

		public bool? IsDisabled { get; internal set; }

		public bool? IsHypothetical { get; internal set; }

		public bool? AllowRowLocks { get; internal set; }

		public bool? AllowPageLocks { get; internal set; }

		public bool? HasFilter { get; internal set; }

		public string FilterDefinition { get; internal set; }
	}
}