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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLiteConversionEngine.Conversion;
using SQLiteConversionEngine.InformationSchema;
using SQLite = SQLiteConversionEngine.InformationSchema.SQLite;
using SqlServer = SQLiteConversionEngine.InformationSchema.SqlServer;

namespace SQLiteConversionEngine.Transform {

    internal class ToSQLiteTableTransform : ToSQLiteTransformBase<SqlServer.Table> {

        public ToSQLiteTableTransform(SqlServer.Table tableToTranslate) : base(tableToTranslate) { }

        //public override SQLite.Table Translate() {
        //    // Note that not all data type names need to be converted because
        //    // SQLite establishes type affinity by searching certain strings
        //    // in the type name. For example - everything containing the string
        //    // 'int' in its type name will be assigned an INTEGER affinity
        //    //if (dataType == "timestamp")
        //    //	dataType = "blob";
        //    //else if (dataType == "datetime" || dataType == "smalldatetime")
        //    //	dataType = "datetime";
        //    //else if (dataType == "decimal")
        //    //	dataType = "numeric";
        //    //else if (dataType == "money" || dataType == "smallmoney")
        //    //	dataType = "numeric";
        //    //else if (dataType == "binary" || dataType == "varbinary" ||
        //    //	dataType == "image")
        //    //	dataType = "blob";
        //    //else if (dataType == "tinyint")
        //    //	dataType = "smallint";
        //    //else if (dataType == "bigint")
        //    //	dataType = "integer";
        //    //else if (dataType == "sql_variant")
        //    //	dataType = "blob";
        //    //else if (dataType == "xml")
        //    //	dataType = "varchar";
        //    //else if (dataType == "uniqueidentifier")
        //    //	dataType = "guid";
        //    //else if (dataType == "ntext")
        //    //	dataType = "text";
        //    //else if (dataType == "nchar")
        //    //	dataType = "char";
        //    //else if (dataType == "datetime2")
        //    //	dataType = "datetime2";
        //    //else if (dataType == "date")
        //    //	dataType = "date";
        //    //else if (dataType == "time")
        //    //	dataType = "time";
        //    //
        //    //if (dataType == "bit" || dataType == "int")
        //    //{
        //    //	if (colDefault == "('False')")
        //    //		colDefault = "(0)";
        //    //	else if (colDefault == "('True')")
        //    //		colDefault = "(1)";
        //    //}
        //    //
        //    //colDefault = FixDefaultValueString(colDefault);

        //    return TranslatedItem;
        //}
    }
}