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
	/// Description of Column.
	/// </summary>
	public class Column : InformationSchemaItemBase<Column> {

		public Column(DataRow itemToLoad) : base(itemToLoad) { }

		protected override void LoadFromDataRow() {
			TableCatalog = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
			TableSchema = OriginalItemDataRow["TABLE_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_SCHEMA"].ToString();
			TableName = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
			ColumnName = OriginalItemDataRow["COLUMN_NAME"] == DBNull.Value ? null : OriginalItemDataRow["COLUMN_NAME"].ToString();
			//TODO: read up on BinaryGUID=False in connectionstring
			ColumnGuid = OriginalItemDataRow["COLUMN_GUID"] == DBNull.Value ? new Nullable<Guid>() : new Guid(OriginalItemDataRow["COLUMN_GUID"].ToString());
			ColumnPropId = OriginalItemDataRow["COLUMN_PROPID"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(OriginalItemDataRow["COLUMN_PROPID"]);
			OrdinalPosition = OriginalItemDataRow["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["ORDINAL_POSITION"]);
			ColumnHasDefault = OriginalItemDataRow["COLUMN_HASDEFAULT"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["COLUMN_HASDEFAULT"].ToString());
			ColumnDefault = OriginalItemDataRow["COLUMN_DEFAULT"] == DBNull.Value ? null : OriginalItemDataRow["COLUMN_DEFAULT"].ToString();
			ColumnFlags = OriginalItemDataRow["COLUMN_FLAGS"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(OriginalItemDataRow["COLUMN_FLAGS"]);
			IsNullable = OriginalItemDataRow["IS_NULLABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["IS_NULLABLE"].ToString());
			DataType = OriginalItemDataRow["DATA_TYPE"] == DBNull.Value ? null : OriginalItemDataRow["DATA_TYPE"].ToString();
			//TODO: read up on BinaryGUID=False in connectionstring
			TypeGuid = OriginalItemDataRow["TYPE_GUID"] == DBNull.Value ? new Nullable<Guid>() : new Guid(OriginalItemDataRow["TYPE_GUID"].ToString());
			CharacterMaximumLength = OriginalItemDataRow["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["CHARACTER_MAXIMUM_LENGTH"]);
			CharacterOctetLength = OriginalItemDataRow["CHARACTER_OCTET_LENGTH"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["CHARACTER_OCTET_LENGTH"]);
			NumericPrecision = OriginalItemDataRow["NUMERIC_PRECISION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["NUMERIC_PRECISION"]);
			NumericScale = OriginalItemDataRow["NUMERIC_SCALE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["NUMERIC_SCALE"]);
			DateTimePrecision = OriginalItemDataRow["DATETIME_PRECISION"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(OriginalItemDataRow["DATETIME_PRECISION"]);
			CharacterSetCatalog = OriginalItemDataRow["CHARACTER_SET_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["CHARACTER_SET_CATALOG"].ToString();
			CharacterSetSchema = OriginalItemDataRow["CHARACTER_SET_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["CHARACTER_SET_SCHEMA"].ToString();
			CharacterSetName = OriginalItemDataRow["CHARACTER_SET_NAME"] == DBNull.Value ? null : OriginalItemDataRow["CHARACTER_SET_NAME"].ToString();
			CollationCatalog = OriginalItemDataRow["COLLATION_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["COLLATION_CATALOG"].ToString();
			CollationSchema = OriginalItemDataRow["COLLATION_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["COLLATION_SCHEMA"].ToString();
			CollationName = OriginalItemDataRow["COLLATION_NAME"] == DBNull.Value ? null : OriginalItemDataRow["COLLATION_NAME"].ToString();
			DomainCatalog = OriginalItemDataRow["DOMAIN_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["DOMAIN_CATALOG"].ToString();
			DomainName = OriginalItemDataRow["DOMAIN_NAME"] == DBNull.Value ? null : OriginalItemDataRow["DOMAIN_NAME"].ToString();
			Description = OriginalItemDataRow["DESCRIPTION"] == DBNull.Value ? null : OriginalItemDataRow["DESCRIPTION"].ToString();
			PrimaryKey = OriginalItemDataRow["PRIMARY_KEY"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["PRIMARY_KEY"].ToString());
			EdmType = OriginalItemDataRow["EDM_TYPE"] == DBNull.Value ? null : OriginalItemDataRow["EDM_TYPE"].ToString();
			AutoIncrement = OriginalItemDataRow["AUTOINCREMENT"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["AUTOINCREMENT"].ToString());
			Unique = OriginalItemDataRow["UNIQUE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["UNIQUE"].ToString());
		}

		protected override void LoadFromObject() {
			throw new NotImplementedException();
		}

		public string TableCatalog { get; private set; }

		public string TableSchema { get; private set; }

		public string TableName { get; private set; }

		public string ColumnName { get; private set; }

		public Guid? ColumnGuid { get; private set; }

		public long? ColumnPropId { get; private set; }

		public int? OrdinalPosition { get; private set; }

		public bool? ColumnHasDefault { get; private set; }

		public string ColumnDefault { get; private set; }

		public long? ColumnFlags { get; private set; }

		public bool? IsNullable { get; private set; }

		public string DataType { get; private set; }

		public Guid? TypeGuid { get; private set; }

		public int? CharacterMaximumLength { get; private set; }

		public int? CharacterOctetLength { get; private set; }

		public int? NumericPrecision { get; private set; }

		public int? NumericScale { get; private set; }

		public long? DateTimePrecision { get; private set; }

		public string CharacterSetCatalog { get; private set; }

		public string CharacterSetSchema { get; private set; }

		public string CharacterSetName { get; private set; }

		public string CollationCatalog { get; private set; }

		public string CollationSchema { get; private set; }

		public string CollationName { get; private set; }

		public string DomainCatalog { get; private set; }

		public string DomainName { get; private set; }

		public string Description { get; private set; }

		public bool? PrimaryKey { get; private set; }

		public string EdmType { get; private set; }

		public bool? AutoIncrement { get; private set; }

		public bool? Unique { get; private set; }
	}
}