using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Configuration;
using System.Text;
using SQLiteConversionEngine.Converters;

namespace SQLiteTest
{
	class Program
	{
		static void Main(string[] args)
		{
			ConvertToSQLite();
			ConvertToSqlServer();
			//using (SQLiteConnection conn = new SQLiteConnection(ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString))
			//{
			//    using (SQLiteCommand comm = new SQLiteCommand("pragma table_info(\"ATPIMAGE\");", conn))
			//    {
			//        conn.Open();
			//        using (SQLiteDataReader rdr = comm.ExecuteReader())
			//        {
			//            while (rdr.Read())
			//            {
			//                StringBuilder sb = new StringBuilder();
			//                for (int i = 0; i < rdr.FieldCount; i++)
			//                {
			//                    sb.AppendFormat("{0}|", rdr.GetValue(i));
			//                }
			//                Console.WriteLine(sb.ToString());
			//            }
			//        }
			//        conn.Close();
			//    }
			//}
		}

		private static void ConvertToSqlServer()
		{
			//SQLiteConverter sqlConv = new SQLiteConverter(Settings.TablesToLoad);

			//using (SqlConnection sqlonn = sqlConv.InitializeSqlServerConnection(Settings.SQLConnectionString))
			//{
			//    bool loadSuccess = false;
			//    SqlConversionHandler sqlConversionHandler = new SqlConversionHandler(
			//            delegate(bool done, bool success, int percent, string msg)
			//            {
			//                if (done)
			//                {
			//                    if (success)
			//                    {
			//                        // log success								
			//                        //Logging.Log(LogLevel.Debug, string.Format("SQLite DB load succeeded: {0}", msg));
			//                        loadSuccess = true;
			//                    }
			//                    else
			//                    {
			//                        // log failure
			//                        //Logging.Log(LogLevel.Error, string.Format("SQLite DB load failed: {0}", msg));
			//                        loadSuccess = false;
			//                    }
			//                }
			//            });
			//    sqlConv.ConvertToDatabase
			//}
		}

		private static void ConvertToSQLite()
		{
			SqlServerToSQLiteConverter sqlConv = new SqlServerToSQLiteConverter(Settings.TablesToLoad);

			using (SQLiteConnection sqliteConn = sqlConv.InitializeSQLiteConnection(Settings.SQLiteConnectionString))
			{
				sqlConv.InitializeSQLiteDatabase(sqliteConn, Settings.PragmaParameters);
				bool loadSuccess = false;
				SqlConversionHandler sqlConversionHandler = new SqlConversionHandler(
						delegate(bool done, bool success, int percent, string msg)
						{
							if (done)
							{
								if (success)
								{
									// log success								
									//Logging.Log(LogLevel.Debug, string.Format("SQLite DB load succeeded: {0}", msg));
									loadSuccess = true;
								}
								else
								{
									// log failure
									//Logging.Log(LogLevel.Error, string.Format("SQLite DB load failed: {0}", msg));
									loadSuccess = false;
								}
							}
						});

				sqlConv.ConvertToDatabase(Settings.SQLConnectionString, sqliteConn, sqlConversionHandler, null, null, false);
				sqliteConn.Close();
				Console.WriteLine(loadSuccess);
			}
		}
	}
}
