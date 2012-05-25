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
    /// Description of View.
    /// </summary>
    public class View : InformationSchemaItemBase<View> {

        public View() {
            ViewColumns = new ViewColumnCollection();
        }

        public View(DataRow itemToLoad)
            : base(itemToLoad) {
            ViewColumns = new ViewColumnCollection();
        }

        protected override void LoadFromDataRow() {
            TableCatalog = OriginalItemDataRow["TABLE_CATALOG"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_CATALOG"].ToString();
            TableSchema = OriginalItemDataRow["TABLE_SCHEMA"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_SCHEMA"].ToString();
            TableName = OriginalItemDataRow["TABLE_NAME"] == DBNull.Value ? null : OriginalItemDataRow["TABLE_NAME"].ToString();
            ViewDefinition = OriginalItemDataRow["VIEW_DEFINITION"] == DBNull.Value ? null : OriginalItemDataRow["VIEW_DEFINITION"].ToString();
            CheckOption = OriginalItemDataRow["CHECK_OPTION"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["CHECK_OPTION"].ToString());
            IsUpdatable = OriginalItemDataRow["IS_UPDATABLE"] == DBNull.Value ? new Nullable<bool>() : Utilities.BoolParser.GetValue(OriginalItemDataRow["IS_UPDATABLE"].ToString());
            Description = OriginalItemDataRow["DESCRIPTION"] == DBNull.Value ? null : OriginalItemDataRow["DESCRIPTION"].ToString();
            DateCreated = OriginalItemDataRow["DATE_CREATED"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(OriginalItemDataRow["DATE_CREATED"]);
            DateModified = OriginalItemDataRow["DATE_MODIFIED"] == DBNull.Value ? new Nullable<DateTime>() : Convert.ToDateTime(OriginalItemDataRow["DATE_MODIFIED"]);
        }

        protected override void LoadFromObject() {
            throw new NotImplementedException();
        }

        public string TableCatalog { get; set; }

        public string TableSchema { get; set; }

        public string TableName { get; set; }

        public string ViewDefinition { get; set; }

        public bool? CheckOption { get; set; }

        public bool? IsUpdatable { get; set; }

        public string Description { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public ViewColumnCollection ViewColumns { get; set; }
    }
}