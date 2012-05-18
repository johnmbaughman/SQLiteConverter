﻿#region

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
	/// Description of ForeignKey.
	/// </summary>
	public class ForeignKey<O> : InformationSchemaItemBase<ForeignKey<O>, O> {

		public ForeignKey(DataRow itemToLoad) : base(itemToLoad) { }

		public string ConstraintCatalog { get; set; }

		public string ConstraintSchema { get; set; }

		public string ConstraintName { get; set; }

		public string TableCatalog { get; set; }

		public string TableSchema { get; set; }

		public string TableName { get; set; }

		public string ConstraintType { get; set; }

		public bool? IsDeferrable { get; set; }

		public bool? InitiallyDeferred { get; set; }

		public string FKeyFromColumn { get; set; }

		public int? FKeyFromOrdinalPosition { get; set; }

		public string FKeyToCatalog { get; set; }

		public string FKeyToSchema { get; set; }

		public string FKeyToTable { get; set; }

		public string FKeyToColumn { get; set; }

		public string FKeyOnUpdate { get; set; }

		public string FKeyOnDelete { get; set; }

		public string FKeyMatch { get; set; }
	}
}