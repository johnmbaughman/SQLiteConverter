using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SQLiteConversionEngine.Conversion;
using SQLiteConversionEngine.InformationSchema;

namespace SqlServerConverter {
	internal class ToSQLiteConversion : ToSQLiteConversionBase {

		public ToSQLiteConversion(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) : base(sqliteConnectionStringSettings, otherConnectionStringSettings) { }

		protected override void ReadSourceSchema(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler) {
			throw new NotImplementedException();
		}

		protected override string BuildSourceTableQuery(Table table) {
			throw new NotImplementedException();
		}

		protected override Table CreateTableSchema(string tableName, string schemaName) {
			throw new NotImplementedException();
		}

		protected override void CreateForeignKeySchema(Table table) {
			throw new NotImplementedException();
		}
	}
}