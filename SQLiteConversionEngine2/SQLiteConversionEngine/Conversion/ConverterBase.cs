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

using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using SQLiteConversionEngine.InformationSchema.SQLite;

namespace SQLiteConversionEngine.Conversion {
	public abstract class ConverterBase<O> {
		internal const string SQLITE_CONNECTION = "sqlite";
		internal const string OTHER_CONNECTION = "other";

		public ConverterBase(string sqliteFileWithPath, string otherServerConnectionString, PragmaCollection<O> pragmaParameters) {
			SQLiteConnectionStringSettings = CreateConnectionStringSettings(sqliteFileWithPath, true);
			OtherConnectionStringSettings = CreateConnectionStringSettings(otherServerConnectionString);
			TablesToLoad = new List<string>();
			SchemasToLoad = new List<string>();
		}

		public List<string> TablesToLoad { get; set; }

		public List<string> SchemasToLoad { get; set; }

		protected ConnectionStringSettings SQLiteConnectionStringSettings { get; private set; }

		protected ConnectionStringSettings OtherConnectionStringSettings { get; private set; }

		public abstract bool ConvertFromSQLite();

		public abstract bool ConvertToSQLite();

		private ConnectionStringSettings CreateConnectionStringSettings(string connectionString, bool isSQLite = false) {
			if (isSQLite) {
				SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder {
					DataSource = connectionString
				};
				return new ConnectionStringSettings(SQLITE_CONNECTION, builder.ConnectionString);
			}
			else {
				return new ConnectionStringSettings(OTHER_CONNECTION, connectionString);
			}
		}
	}
}