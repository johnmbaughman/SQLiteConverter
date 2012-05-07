using System.Collections.Generic;

namespace SQLiteConversionEngine.InformationSchema {
	public class Schema : InformationSchemaBase {

		public Schema() {
			Database = new Database();
			Tables = new List<Table>();
			Views = new List<View>();
		}

		public Database Database { get; set; }

		public List<Table> Tables { get; set; }

		public List<View> Views { get; set; }
	}
}