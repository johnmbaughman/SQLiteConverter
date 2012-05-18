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
	public class ForeignKey : InformationSchemaItemBase<ForeignKey, SQLite.ForeignKey<ForeignKey>> {

		public ForeignKey(DataRow itemToLoad)
			: base(itemToLoad) {
			ConstraintCatalog = itemToLoad["CONSTRAINT_CATALOG"] == DBNull.Value ? null : itemToLoad["CONSTRAINT_CATALOG"].ToString();
			ConstraintSchema = itemToLoad["CONSTRAINT_SCHEMA"] == DBNull.Value ? null : itemToLoad["CONSTRAINT_SCHEMA"].ToString();
			ConstraintName = itemToLoad["CONSTRAINT_NAME"] == DBNull.Value ? null : itemToLoad["CONSTRAINT_NAME"].ToString();
			TableCatalog = itemToLoad["TABLE_CATALOG"] == DBNull.Value ? null : itemToLoad["TABLE_CATALOG"].ToString();
			TableSchema = itemToLoad["TABLE_SCHEMA"] == DBNull.Value ? null : itemToLoad["TABLE_SCHEMA"].ToString();
			TableName = itemToLoad["TABLE_NAME"] == DBNull.Value ? null : itemToLoad["TABLE_NAME"].ToString();
			ConstraintType = itemToLoad["CONSTRAINT_TYPE"] == DBNull.Value ? null : itemToLoad["CONSTRAINT_TYPE"].ToString();
			IsDeferrable = itemToLoad["IS_DEFERRABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(itemToLoad["IS_DEFERRABLE"].ToString());
			InitiallyDeferred = itemToLoad["INITIALLY_DEFERRED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(itemToLoad["INITIALLY_DEFERRED"].ToString());
		}

		public string ConstraintCatalog { get; internal set; }

		public string ConstraintSchema { get; internal set; }

		public string ConstraintName { get; internal set; }

		public string TableCatalog { get; internal set; }

		public string TableSchema { get; internal set; }

		public string TableName { get; internal set; }

		public string ConstraintType { get; internal set; }

		public bool? IsDeferrable { get; internal set; }

		public bool? InitiallyDeferred { get; internal set; }
	}
}