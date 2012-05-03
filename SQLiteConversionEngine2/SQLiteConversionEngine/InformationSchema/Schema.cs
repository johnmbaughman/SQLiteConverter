using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteConversionEngine.InformationSchema {
	public class Schema : InformationSchemaBase {

		public Database Database { get; set; }

		public List<Table> Tables { get; set; }

		public List<View> Views { get; set; }
	}
}