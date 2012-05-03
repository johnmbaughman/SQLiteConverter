using System.Data.SqlClient;
using sql = SqlServerConverter;

namespace ConversionTestConsole {
	internal class Program {

		private static void Main(string[] args) {
			sql.Converter.ConvertFromSqlServer(new sql.FromSqlConnectionInformation());
			sql.Converter.ConvertToSqlServer(new sql.ToSqlConnectionInformation());
		}
	}
}