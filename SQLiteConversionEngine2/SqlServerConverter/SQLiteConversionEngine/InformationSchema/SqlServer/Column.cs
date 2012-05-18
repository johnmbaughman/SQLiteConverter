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
	public class Column : InformationSchemaItemBase<Column, SQLite.Column<Column>> {

		public Column(DataRow itemToLoad)
			: base(itemToLoad) {
			TableCatalog = itemToLoad["TABLE_CATALOG"] == DBNull.Value ? null : itemToLoad["TABLE_CATALOG"].ToString();
			TableSchema = itemToLoad["TABLE_SCHEMA"] == DBNull.Value ? null : itemToLoad["TABLE_SCHEMA"].ToString();
			TableName = itemToLoad["TABLE_NAME"] == DBNull.Value ? null : itemToLoad["TABLE_NAME"].ToString();
			ColumnName = itemToLoad["COLUMN_NAME"] == DBNull.Value ? null : itemToLoad["COLUMN_NAME"].ToString();
			OrdinalPosition = itemToLoad["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["ORDINAL_POSITION"]);
			ColumnDefault = itemToLoad["COLUMN_DEFAULT"] == DBNull.Value ? null : itemToLoad["COLUMN_DEFAULT"].ToString();
			IsNullable = itemToLoad["IS_NULLABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(itemToLoad["IS_NULLABLE"].ToString());
			DataType = itemToLoad["DATA_TYPE"] == DBNull.Value ? null : itemToLoad["DATA_TYPE"].ToString();
			CharacterMaximumLength = itemToLoad["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["CHARACTER_MAXIMUM_LENGTH"]);
			CharacterOctetLength = itemToLoad["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["CHARACTER_OCTET_LENGTH"]);
			NumericPrecision = itemToLoad["NUMERIC_PRECISION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["NUMERIC_PRECISION"]);
			NumericPrecisionRadix = itemToLoad["NUMERIC_PRECISION_RADIX"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["NUMERIC_PRECISION_RADIX"]);
			NumericScale = itemToLoad["NUMERIC_SCALE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(itemToLoad["NUMERIC_SCALE"]);
			DateTimePrecision = itemToLoad["DATETIME_PRECISION"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt32(itemToLoad["DATETIME_PRECISION"]);
			CharacterSetCatalog = itemToLoad["CHARACTER_SET_CATALOG"] == DBNull.Value ? null : itemToLoad["CHARACTER_SET_CATALOG"].ToString();
			CharacterSetSchema = itemToLoad["CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : itemToLoad["CHARACTER_SET_SCHEMA"].ToString();
			CharacterSetName = itemToLoad["CHARACTER_SET_NAME"] == DBNull.Value ? null : itemToLoad["CHARACTER_SET_NAME"].ToString();
			CollationCatalog = itemToLoad["COLLATION_CATALOG"] == DBNull.Value ? null : itemToLoad["COLLATION_CATALOG"].ToString();
			IsSparse = itemToLoad["IS_SPARSE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(itemToLoad["IS_SPARSE"].ToString());
			IsColumnSet = itemToLoad["IS_COLUMN_SET"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(itemToLoad["IS_COLUMN_SET"].ToString());
			IsFileStream = itemToLoad["IS_FILESTREAM"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(itemToLoad["IS_FILESTREAM"].ToString());
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