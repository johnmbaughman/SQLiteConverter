using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using SQLiteConversionEngine.Conversion;

namespace SqlServerConverter {
	internal class FromSQLiteConversion : FromSQLiteConversionBase {

		public FromSQLiteConversion(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) : base(sqliteConnectionStringSettings, otherConnectionStringSettings) { }

		//protected override void ReadSourceSchema(ConversionBase.ConversionHandler conversionHandler, ConversionBase.TableSelectionHandler tableSelectionHandler) {
		//    throw new NotImplementedException();
		//}
	}
}