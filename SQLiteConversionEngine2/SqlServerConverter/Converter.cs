using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SqlServerConverter {
	public class Converter {

		public static void ConvertToSqlServer(ToSqlConnectionInformation toSqlConnectionInformation) {
			ConvertToSqlServer<SqlConnection, SqlCommand, SqlConnection, SqlCommand> convert = new ConvertToSqlServer<SqlConnection, SqlCommand, SqlConnection, SqlCommand>();
		}

		public static void ConvertFromSqlServer(FromSqlConnectionInformation fromSqlConnectionInformation) {
			ConvertToSqlServer<SqlConnection, SqlCommand, SqlConnection, SqlCommand> convert = new ConvertToSqlServer<SqlConnection, SqlCommand, SqlConnection, SqlCommand>();
		}
	}
}