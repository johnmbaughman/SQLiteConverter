#region

// -----------------------------------------------------
// MIT License
// Copyright (C) 2012 John M. Baughman (jbaughmanphoto.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"); to deal in the Software without restriction,
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
	public class ForeignKey : InformationSchemaItemBase<ForeignKey> {

		public ForeignKey(DataRow itemToLoad) : base(itemToLoad) { }

		protected override void LoadFromDataRow() {
			ConstraintCatalog = OriginalItemDataRow["CONSTRAINT_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["CONSTRAINT_CATALOG"].ToString();
			ConstraintSchema = OriginalItemDataRow["CONSTRAINT_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["CONSTRAINT_SCHEMA"].ToString();
			ConstraintName = OriginalItemDataRow["CONSTRAINT_NAME"] == DBNull.Value ? null : OriginalItemDataRow["CONSTRAINT_NAME"].ToString();
			TableCatalog = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
			TableSchema = OriginalItemDataRow["TABLE_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_SCHEMA"].ToString();
			TableName = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
			ConstraintType = OriginalItemDataRow["CONSTRAINT_TYPE"] == DBNull.Value ? null : OriginalItemDataRow["CONSTRAINT_TYPE"].ToString();
			IsDeferrable = OriginalItemDataRow["IS_DEFERRABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["IS_DEFERRABLE"].ToString());
			InitiallyDeferred = OriginalItemDataRow["INITIALLY_DEFERRED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["INITIALLY_DEFERRED"].ToString());
			FKeyFromColumn = OriginalItemDataRow["FKEY_FROM_COLUMN"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_FROM_COLUMN"].ToString();
			FKeyFromOrdinalPosition = OriginalItemDataRow["FKEY_FROM_ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["FKEY_FROM_ORDINAL_POSITION"]);
			FKeyToCatalog = OriginalItemDataRow["FKEY_TO_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_TO_CATALOG"].ToString();
			FKeyToSchema = OriginalItemDataRow["FKEY_TO_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_TO_SCHEMA"].ToString();
			FKeyToTable = OriginalItemDataRow["FKEY_TO_TABLE"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_TO_TABLE"].ToString();
			FKeyToColumn = OriginalItemDataRow["FKEY_TO_COLUMN"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_TO_COLUMN"].ToString();
			FKeyOnUpdate = OriginalItemDataRow["FKEY_ON_UPDATE"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_ON_UPDATE"].ToString();
			FKeyOnDelete = OriginalItemDataRow["FKEY_ON_DELETE"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_ON_DELETE"].ToString();
			FKeyMatch = OriginalItemDataRow["FKEY_MATCH"] == DBNull.Value ? null : OriginalItemDataRow["FKEY_MATCH"].ToString();
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

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