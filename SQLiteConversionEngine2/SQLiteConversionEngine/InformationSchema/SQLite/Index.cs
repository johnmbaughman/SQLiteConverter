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
    /// Description of Index.
    /// </summary>
    public class Index : InformationSchemaItemBase<Index> {

        public Index() {
            IndexColumns = new IndexColumnCollection();
        }

        public Index(DataRow itemToLoad)
            : base(itemToLoad) {
            IndexColumns = new IndexColumnCollection();
        }

        protected override void LoadFromDataRow() {
            TableCatalog = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
            TableSchema = OriginalItemDataRow["TABLE_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_SCHEMA"].ToString();
            TableName = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
            IndexCatalog = OriginalItemDataRow["INDEX_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["INDEX_CATALOG"].ToString();
            IndexSchema = OriginalItemDataRow["INDEX_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["INDEX_SCHEMA"].ToString();
            IndexName = OriginalItemDataRow["INDEX_NAME"] == DBNull.Value ? null : OriginalItemDataRow["INDEX_NAME"].ToString();
            PrimaryKey = OriginalItemDataRow["PRIMARY_KEY"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["PRIMARY_KEY"].ToString());
            Unique = OriginalItemDataRow["UNIQUE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["UNIQUE"].ToString());
            Clustered = OriginalItemDataRow["CLUSTERED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["CLUSTERED"].ToString());
            Type = OriginalItemDataRow["TYPE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["TYPE"]);
            FillFactor = OriginalItemDataRow["FILL_FACTOR"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["FILL_FACTOR"]);
            InitialSize = OriginalItemDataRow["INITIAL_SIZE"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["INITIAL_SIZE"]);
            Nulls = OriginalItemDataRow["NULLS"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["NULLS"]);
            SortBookmarks = OriginalItemDataRow["SORT_BOOKMARKS"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["SORT_BOOKMARKS"].ToString());
            AutoUpdate = OriginalItemDataRow["AUTO_UPDATE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["AUTO_UPDATE"].ToString());
            NullCollation = OriginalItemDataRow["NULL_COLLATION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["NULL_COLLATION"]);
            OrdinalPosition = OriginalItemDataRow["ORDINAL_POSITION"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["ORDINAL_POSITION"]);
            ColumnName = OriginalItemDataRow["COLUMN_NAME"] == DBNull.Value ? null : OriginalItemDataRow["COLUMN_NAME"].ToString();
            //TODO: read up on BinaryGUID=False in connectionstring
            ColumnGUID = OriginalItemDataRow["COLUMN_GUID"] == DBNull.Value ? new Nullable<Guid>() : new Guid(OriginalItemDataRow["COLUMN_GUID"].ToString());
            ColumnPropId = OriginalItemDataRow["COLUMN_PROPID"] == DBNull.Value ? new Nullable<long>() : Convert.ToInt64(OriginalItemDataRow["COLUMN_PROPID"]);
            Collation = OriginalItemDataRow["COLLATION"] == DBNull.Value ? new Nullable<short>() : Convert.ToInt16(OriginalItemDataRow["COLLATION"]);
            Cardinality = OriginalItemDataRow["CARDINALITY"] == DBNull.Value ? new Nullable<decimal>() : Convert.ToDecimal(OriginalItemDataRow["CARDINALITY"]);
            Pages = OriginalItemDataRow["PAGES"] == DBNull.Value ? new Nullable<int>() : Convert.ToInt32(OriginalItemDataRow["PAGES"]);
            FilterCondition = OriginalItemDataRow["FILTER_CONDITION"] == DBNull.Value ? null : OriginalItemDataRow["FILTER_CONDITION"].ToString();
            Integrated = OriginalItemDataRow["INTEGRATED"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["INTEGRATED"].ToString());
            IndexDefinition = OriginalItemDataRow["INDEX_DEFINITION"] == DBNull.Value ? null : OriginalItemDataRow["INDEX_DEFINITION"].ToString();
        }

        protected override void LoadFromObject() {
            throw new NotImplementedException();
        }

        public string TableCatalog { get; set; }

        public string TableSchema { get; set; }

        public string TableName { get; set; }

        public string IndexCatalog { get; set; }

        public string IndexSchema { get; set; }

        public string IndexName { get; set; }

        public bool? PrimaryKey { get; set; }

        public bool? Unique { get; set; }

        public bool? Clustered { get; set; }

        public int? Type { get; set; }

        public int? FillFactor { get; set; }

        public int? InitialSize { get; set; }

        public int? Nulls { get; set; }

        public bool? SortBookmarks { get; set; }

        public bool? AutoUpdate { get; set; }

        public int? NullCollation { get; set; }

        public int? OrdinalPosition { get; set; }

        public string ColumnName { get; set; }

        public Guid? ColumnGUID { get; set; }

        public long? ColumnPropId { get; set; }

        public short? Collation { get; set; }

        public decimal? Cardinality { get; set; }

        public int? Pages { get; set; }

        public string FilterCondition { get; set; }

        public bool? Integrated { get; set; }

        public string IndexDefinition { get; set; }

        public IndexColumnCollection IndexColumns { get; set; }
    }
}