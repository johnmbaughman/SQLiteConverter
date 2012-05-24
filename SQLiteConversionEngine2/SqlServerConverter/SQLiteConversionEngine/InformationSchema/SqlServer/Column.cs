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
	public class Column : InformationSchemaItemBase<Column> {

		public Column(DataRow itemToLoad) : base(itemToLoad) { }

		protected override void LoadFromDataRow() {
			TableCatalog = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
			TableSchema = OriginalItemDataRow["TABLE_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_SCHEMA"].ToString();
			TableName = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
			ColumnName = OriginalItemDataRow["COLUMN_NAME"] == DBNull.Value ? null : OriginalItemDataRow["COLUMN_NAME"].ToString();
			OrdinalPosition = OriginalItemDataRow["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["ORDINAL_POSITION"]);
			ColumnDefault = OriginalItemDataRow["COLUMN_DEFAULT"] == DBNull.Value ? null : OriginalItemDataRow["COLUMN_DEFAULT"].ToString();
			IsNullable = OriginalItemDataRow["IS_NULLABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["IS_NULLABLE"].ToString());
			DataType = OriginalItemDataRow["DATA_TYPE"] == DBNull.Value ? null : OriginalItemDataRow["DATA_TYPE"].ToString();
			CharacterMaximumLength = OriginalItemDataRow["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["CHARACTER_MAXIMUM_LENGTH"]);
			CharacterOctetLength = OriginalItemDataRow["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["CHARACTER_OCTET_LENGTH"]);
			NumericPrecision = OriginalItemDataRow["NUMERIC_PRECISION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["NUMERIC_PRECISION"]);
			NumericPrecisionRadix = OriginalItemDataRow["NUMERIC_PRECISION_RADIX"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["NUMERIC_PRECISION_RADIX"]);
			NumericScale = OriginalItemDataRow["NUMERIC_SCALE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["NUMERIC_SCALE"]);
			DateTimePrecision = OriginalItemDataRow["DATETIME_PRECISION"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt32(OriginalItemDataRow["DATETIME_PRECISION"]);
			CharacterSetCatalog = OriginalItemDataRow["CHARACTER_SET_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["CHARACTER_SET_CATALOG"].ToString();
			CharacterSetSchema = OriginalItemDataRow["CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["CHARACTER_SET_SCHEMA"].ToString();
			CharacterSetName = OriginalItemDataRow["CHARACTER_SET_NAME"] == DBNull.Value ? null : OriginalItemDataRow["CHARACTER_SET_NAME"].ToString();
			CollationCatalog = OriginalItemDataRow["COLLATION_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["COLLATION_CATALOG"].ToString();
			IsSparse = OriginalItemDataRow["IS_SPARSE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["IS_SPARSE"].ToString());
			IsColumnSet = OriginalItemDataRow["IS_COLUMN_SET"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["IS_COLUMN_SET"].ToString());
			IsFileStream = OriginalItemDataRow["IS_FILESTREAM"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["IS_FILESTREAM"].ToString());
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

		public string TableCatalog { get; private set; }

		public string TableSchema { get; private set; }

		public string TableName { get; private set; }

		public string ColumnName { get; private set; }

		public int? OrdinalPosition { get; private set; }

		public string ColumnDefault { get; private set; }

		public bool? IsNullable { get; private set; }

		public string DataType { get; private set; }

		public int? CharacterMaximumLength { get; private set; }

		public int? CharacterOctetLength { get; private set; }

		public int? NumericPrecision { get; private set; }

		public int? NumericPrecisionRadix { get; private set; }

		public int? NumericScale { get; private set; }

		public long? DateTimePrecision { get; private set; }

		public string CharacterSetCatalog { get; private set; }

		public string CharacterSetSchema { get; private set; }

		public string CharacterSetName { get; private set; }

		public string CollationCatalog { get; private set; }

		public bool? IsSparse { get; private set; }

		public bool? IsColumnSet { get; private set; }

		public bool? IsFileStream { get; private set; }
	}
}