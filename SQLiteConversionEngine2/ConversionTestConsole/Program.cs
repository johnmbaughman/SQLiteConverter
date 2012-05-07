using System.Configuration;
using System.Data.SqlClient;
using sql = SqlServerConverter;

namespace ConversionTestConsole {
	internal class Program {

		private static void Main(string[] args) {
			sql.Converter converter = new sql.Converter("C:\\SQLiteTest.db", "Data Source=localhost;Initial Catalog=SQLiteConversion;Integrated Security=True", null);
			converter.TablesToLoad.AddRange(new string[] { "Table_1", "Table_2" });
			converter.SchemasToLoad.Add("dbo");
			converter.ConvertToSQLite();
			//converter.ConvertFromSQLite();
		}
	}
}