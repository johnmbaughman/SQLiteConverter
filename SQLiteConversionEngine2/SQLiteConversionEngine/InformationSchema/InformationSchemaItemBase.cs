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
using System.Dynamic;
using System.Reflection;
using System.Text;
using SQLiteConversionEngine.Dynamic;

namespace SQLiteConversionEngine.InformationSchema {

    public abstract class InformationSchemaItemBase<T> : GenericDynamicModel<T> {

        public InformationSchemaItemBase() { }

        public InformationSchemaItemBase(DataRow itemToLoad) {
            OriginalItemDataRow = itemToLoad;
            LoadFromDataRow();
        }

        public InformationSchemaItemBase(T itemToLoad) {
            OriginalItemObject = itemToLoad;
            LoadFromObject();
        }

        protected DataRow OriginalItemDataRow { get; private set; }

        protected T OriginalItemObject { get; private set; }

        protected abstract void LoadFromDataRow();

        protected abstract void LoadFromObject();

        public override string ToString() {
            StringBuilder sc = new StringBuilder();

            foreach (PropertyInfo propertyItem in this.GetType().GetProperties()) {
                string propName = propertyItem.Name.ToString();
                var tempVal = propertyItem.GetValue(this, null);
                var propVal = tempVal == null ? string.Empty : tempVal;

                sc.AppendFormat("{0} : {1}{2}", propName, propVal, Environment.NewLine);
            }

            return sc.ToString();
        }
    }
}