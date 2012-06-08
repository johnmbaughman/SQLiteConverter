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

namespace SQLiteConversionEngine.InformationSchema.SqlServer {
	public class Trigger : InformationSchemaItemBase<Trigger, SQLite.Trigger<Trigger>> {

		public Trigger(DataRow itemToLoad) : base(itemToLoad) { }

		public string Name { get; internal set; }

		public int? ObjectId { get; internal set; }

		public int? ParentClass { get; internal set; }

		public string ParentClassDescription { get; internal set; }

		public int? ParentId { get; internal set; }

		public string Type { get; internal set; }

		public string TypeDescription { get; internal set; }

		public DateTime? CreateDate { get; internal set; }

		public DateTime? ModifyDate { get; internal set; }

		public bool? IsMsShipped { get; internal set; }

		public bool? IsDisabled { get; internal set; }

		public bool? IsNotForReplication { get; internal set; }

		public bool? IsInsteadOfTrigger { get; internal set; }

		public override void LoadItem(DataRow itemToLoad) {
			throw new NotImplementedException();
		}
	}
}