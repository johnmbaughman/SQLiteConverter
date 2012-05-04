using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using SQLiteConversionEngine.InformationSchema;
using SQLiteConversionEngine.Utility;

namespace SQLiteConversionEngine.Conversion {
	public abstract class FromSQLiteConversionBase : ConversionBase {

		public FromSQLiteConversionBase(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) : base(sqliteConnectionStringSettings, otherConnectionStringSettings) { }

		protected override string BuildSourceTableQuery(InformationSchema.Table table) {
			throw new NotImplementedException();
		}

		protected override void ConvertSourceDatabaseToDestination(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			throw new NotImplementedException();
		}

		public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			throw new NotImplementedException();
		}

		protected override void CopySourceDataToDestination(ConversionHandler conversionHandler) {
			throw new NotImplementedException();
		}

		protected override void CreateForeignKeySchema(Table table) {
			throw new NotImplementedException();
		}

		protected override Table CreateTableSchema(string tableName, string schemaName) {
			throw new NotImplementedException();
		}

		protected override void ReadSourceSchema(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler) {
			throw new NotImplementedException();
		}
	}
}