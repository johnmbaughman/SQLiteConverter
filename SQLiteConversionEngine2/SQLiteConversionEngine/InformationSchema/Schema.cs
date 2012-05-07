using System.Collections.Generic;

namespace SQLiteConversionEngine.InformationSchema {
	public class Schema : InformationSchemaBase {

		public Database Database { get; set; }

		public List<Table> Tables { get; set; }

		public List<View> Views { get; set; }
	}
}