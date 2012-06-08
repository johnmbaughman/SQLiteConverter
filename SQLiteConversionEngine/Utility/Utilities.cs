using SQLiteConversionEngine.ConversionData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteConversionEngine.Utility
{
	internal class Utilities
	{
		public static bool ContainsSchemaInfo(List<TableToLoad> list)
		{
			bool returnVal = false;
			list.ForEach(item => returnVal = !returnVal ? returnVal == string.IsNullOrEmpty(item.SqlServerSchemaName) : returnVal);
			return returnVal;
		}

		public static List<string> ConvertListToCase(List<string> list, bool upperCase)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = upperCase ? list[i].ToUpper() : list[i].ToLower();
			}
			return list;
		}
		
		public static string ConvertTableToLoadToInClause(List<TableToLoad> list, bool isSchemaList, string tableName)
		{
			try
			{
				string returnString = string.Format(" {0} IN (|", tableName);
				foreach (var item in list)
				{
					string name = isSchemaList ? item.SqlServerSchemaName : item.SqlServerTableName;
					if (!returnString.Contains(name))
					{
						returnString += string.Format(", '{0}'", name);
					}
				}
				returnString += ")";
				returnString = returnString.Replace("|,", "");

				return returnString;
			}
			catch (Exception ex)
			{
				//Logging.Log(LogLevel.Error, string.Format("ConvertListToInClause exception: {0}", LoggingEngine.FileLogger.GetInnerException(ex).ToString()));
				return string.Empty;
			}
		}

		public static void PrintListToLog(List<string> list)
		{
			foreach (var item in list)
			{
				//Logging.Log(LogLevel.Debug, "\t" + item);
			}
		}
	}
}
