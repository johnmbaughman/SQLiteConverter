using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteConversionEngine.ConversionData
{
	public class TableToLoad
	{
		
		public string SqlServerSchemaName { get; set; }
		public string SQLiteTableName { get; set; }
		public string SqlServerTableName { get; set; }
		public DataActionEnum SQLiteDataAction { get; set; }
		public DataActionEnum SqlServerDataAction { get; set; }
		public string SqlServerFullName
		{
			get
			{
				return SqlServerSchemaName + "." + SqlServerTableName;
			}
		}		
	}
}
