using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SQLiteConversionEngine.Conversion;
using SQLiteConversionEngine.InformationSchema;

namespace SqlServerConverter {
	public class Converter : ConverterBase {

		public Converter(string sqliteFileWithPath, string SqlServerConnectionString, List<Pragma> pragmaParameters)
			: base(sqliteFileWithPath, SqlServerConnectionString, pragmaParameters) { }

		public override bool ConvertToSQLite() {
			ToSQLiteConversion toSQLiteConversion = new ToSQLiteConversion(SQLiteConnectionStringSettings, OtherConnectionStringSettings);
			return true;
		}

		public override bool ConvertFromSQLite() {
			FromSQLiteConversion fromSQLiteConversion = new FromSQLiteConversion(SQLiteConnectionStringSettings, OtherConnectionStringSettings);
			fromSQLiteConversion.ConvertToDatabase(null, null, null, false);
			return true;
		}
	}
}