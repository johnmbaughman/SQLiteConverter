using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteConversionEngine.InformationSchema {
	public class Table : InformationSchemaBase {

		public List<Column> Columns { get; set; }

		public List<Index> Indexes { get; set; }

		public List<ForeignKey> ForeignKeys { get; set; }

		public List<Trigger> Triggers { get; set; }

		public string PrimaryKey { get; set; }
	}
}