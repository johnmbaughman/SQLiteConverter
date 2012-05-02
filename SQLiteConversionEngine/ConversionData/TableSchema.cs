using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteConversionEngine.ConversionData
{
	public class TableSchema
	{
		public string TableName { get; set; }
		public string TableSchemaName { get; set; }
		public List<ColumnSchema> Columns { get; set; }
		public List<string> PrimaryKey { get; set; }
		public List<ForeignKeySchema> ForeignKeys { get; set; }
		public List<IndexSchema> Indexes { get; set; }
		public DataActionEnum DataAction { get; set; }
	}
}
