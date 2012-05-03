using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SQLiteConversionEngine.Conversion {
	public interface IConnectionInformation {

		string SourceConnectionString { get; set; }

		string DestinationConnectionString { get; set; }
	}

	public interface IConnectionInformation<sCN, dCN> : IConnectionInformation {

		sCN SourceConnection { get; set; }

		dCN DestinationConnection { get; set; }
	}
}