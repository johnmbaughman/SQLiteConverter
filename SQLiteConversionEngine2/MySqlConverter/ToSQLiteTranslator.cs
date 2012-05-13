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
using SQLiteConversionEngine.InformationSchema;
using MySql = SQLiteConversionEngine.InformationSchema.MySql;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;

namespace MySqlConverter
{
    internal class ToSQLiteTranslator : InformationSchemaTranslator<MySql.Database, SQLite.Database>
    {
        private SQLite.Database currentDatabase = null;
        private SQLite.Catalog currentCatalog = null;
        private SQLite.Index currentIndex = null;
        private SQLite.Table currentTable = null;
        private SQLite.View currentView = null;

        public ToSQLiteTranslator(MySql.Database MySqlDatabase) : base(MySqlDatabase) { }

        public override SQLite.Database Translate()
        {
            throw new NotImplementedException();
        }

        //protected override void TranslateCatalog() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateColumns() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateForeignKeys() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateIndexColumns() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateIndexes() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateTables() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateTriggers() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateViewColumns() {
        //    throw new NotImplementedException();
        //}

        //protected override void TranslateViews() {
        //    throw new NotImplementedException();
        //}
    }
}