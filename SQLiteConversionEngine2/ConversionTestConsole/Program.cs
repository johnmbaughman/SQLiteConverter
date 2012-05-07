using System.Configuration;
using System.Data.SqlClient;
using sql = SqlServerConverter;

namespace ConversionTestConsole {
	internal class Program {

		private static void Main(string[] args) {
			sql.Converter converter = new sql.Converter("C:\\SQLiteTest.db", "Data Source=localhost;Initial Catalog=CartonLabelWS12;Integrated Security=True", null);
			//converter.ConvertToSQLite();
			converter.ConvertFromSQLite();
		}
	}
}