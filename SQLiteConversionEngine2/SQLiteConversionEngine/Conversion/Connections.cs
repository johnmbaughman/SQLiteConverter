using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace SQLiteConversionEngine.Conversion {
	internal class Connections {

		public ConnectionStringSettings SQLiteConnection { get; set; }

		public ConnectionStringSettings OtherConnection { get; set; }
	}
}