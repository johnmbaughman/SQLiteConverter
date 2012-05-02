using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Resources;
using System.Text;
using SQLiteConversionEngine.ConversionData;

namespace SQLiteTest
{
	class Settings
	{
		private static string resourceName = "SQLiteTest.Properties.Resources";
		private static string pragmaPrefix = "PRAGMA_";
		private static string tablesToLoadPrefix = "TABLE_";
				
		public static string SQLConnectionString
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
			}
		}

		public static string SQLiteConnectionString
		{
			get
			{
				return ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString;
			}
		}

		public static List<string[]> PragmaParameters
		{
			get
			{
				return GetResourceList(pragmaPrefix, '|');
			}
		}
						
		public static List<TableToLoad> TablesToLoad
		{
			get
			{
				List<string> tablesToLoad = GetResourceList(tablesToLoadPrefix);
				List<TableToLoad> returnList = new List<TableToLoad>();
				foreach (var table in tablesToLoad)
				{
					string[] tableSplit = table.Split('|');
					string[] sqlServer = tableSplit[2].Split('.');
					returnList.Add( new TableToLoad(){
						SQLiteTableName = tableSplit[0],
						SQLiteDataAction =  (DataActionEnum)Enum.Parse(typeof(DataActionEnum), tableSplit[1]),
						SqlServerSchemaName = sqlServer.Length == 2 ? sqlServer[0] : "dbo",
						SqlServerTableName = sqlServer.Length == 2 ? sqlServer[1] : sqlServer[0],
						SqlServerDataAction = (DataActionEnum)Enum.Parse(typeof(DataActionEnum), tableSplit[3])
					});
				}				
				return returnList;
			}
		}
		
		private static ResourceManager GetResourceManager()
		{
			return new ResourceManager(resourceName, Assembly.GetExecutingAssembly());
		}

		private static List<string> GetResourceList(string resourcePrefix)
		{
			List<string> returnCommands = new List<string>();

			ResourceManager rm = GetResourceManager();

			int count = 1;

			while (!string.IsNullOrEmpty(rm.GetString(resourcePrefix + count.ToString())))
			{
				returnCommands.Add(rm.GetString(resourcePrefix + count.ToString()));
				count++;
			}

			return returnCommands;
		}

		private static List<string[]> GetResourceList(string resourcePrefix, char delimiter)
		{
			List<string[]> returnCommands = new List<string[]>();

			ResourceManager rm = GetResourceManager();

			int count = 1;

			while (!string.IsNullOrEmpty(rm.GetString(resourcePrefix + count.ToString())))
			{
				returnCommands.Add(rm.GetString(resourcePrefix + count.ToString()).Split(delimiter));
				count++;
			}
			return returnCommands;
		}

		private static Dictionary<string, string> GetResourceList(string resourcePrefix, List<string> names)
		{
			Dictionary<string, string> returnCommands = new Dictionary<string, string>();

			ResourceManager rm = GetResourceManager();

			foreach (var name in names)
			{
				if (!string.IsNullOrEmpty(rm.GetString(resourcePrefix + name)))
				{
					returnCommands.Add(name, rm.GetString(resourcePrefix + name));
				}
			}

			return returnCommands;
		}

		private static string GetResourceString(string resourceId)
		{
			ResourceManager rm = GetResourceManager();
			if (!string.IsNullOrEmpty(rm.GetString(resourceId)))
			{
				return rm.GetString(resourceId);
			}
			return string.Empty;
		}
	}
}
