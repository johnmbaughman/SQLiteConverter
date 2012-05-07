using System.Collections.Generic;

namespace SQLiteConversionEngine.InformationSchema {
	public class Table : InformationSchemaBase {

		public Table() {
			Columns = new List<Column>();
			Indexes = new List<Index>();
			ForeignKeys = new List<ForeignKey>();
			Triggers = new List<Trigger>();
		}

		public List<Column> Columns { get; set; }

		public List<Index> Indexes { get; set; }

		public List<ForeignKey> ForeignKeys { get; set; }

		public List<Trigger> Triggers { get; set; }

		public string PrimaryKey { get; set; }
	}
}