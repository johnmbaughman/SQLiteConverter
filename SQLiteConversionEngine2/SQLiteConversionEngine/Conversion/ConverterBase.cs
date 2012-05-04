using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using SQLiteConversionEngine.InformationSchema;

namespace SQLiteConversionEngine.Conversion {
	public abstract class ConverterBase {
		internal const string SQLITE_CONNECTION = "sqlite";
		internal const string OTHER_CONNECTION = "other";

		public ConverterBase(string sqliteFileWithPath, string otherServerConnectionString, List<Pragma> pragmaParameters) {
			SQLiteConnectionStringSettings = CreateConnectionStringSettings(sqliteFileWithPath, true);
			OtherConnectionStringSettings = CreateConnectionStringSettings(otherServerConnectionString);
		}

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