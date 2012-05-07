using System.Collections.Generic;

namespace SQLiteConversionEngine.InformationSchema {
	public class Table : InformationSchemaBase {

		public List<Column> Columns { get; set; }

		public List<Index> Indexes { get; set; }

		public List<ForeignKey> ForeignKeys { get; set; }

		public List<Trigger> Triggers { get; set; }

		public string PrimaryKey { get; set; }
	}
}