using System;
using System.Configuration;
using SQLiteConversionEngine.InformationSchema;

namespace SQLiteConversionEngine.Conversion {
	public abstract class ToSQLiteConversionBase : ConversionBase {

		public ToSQLiteConversionBase(ConnectionStringSettings sqliteConnectionStringSettings, ConnectionStringSettings otherConnectionStringSettings) : base(sqliteConnectionStringSettings, otherConnectionStringSettings) { }

		protected abstract override string BuildSourceTableQuery(InformationSchema.Table table);

		protected override void ConvertSourceDatabaseToDestination(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			throw new NotImplementedException();
		}

		public override void ConvertToDatabase(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler, FailedViewDefinitionHandler failedViewDefinitionHandler, bool createTriggers) {
			throw new NotImplementedException();
		}

		protected override void CopySourceDataToDestination(ConversionHandler conversionHandler) {
			throw new NotImplementedException();
		}

		protected abstract override void CreateForeignKeySchema(Table table);

		protected abstract override Table CreateTableSchema(string tableName, string schemaName);

		protected abstract override void ReadSourceSchema(ConversionHandler conversionHandler, TableSelectionHandler tableSelectionHandler);
	}
}