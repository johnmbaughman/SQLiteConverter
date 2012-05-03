using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SQLiteConversionEngine.Conversion;

namespace SqlServerConverter {
	public class ToSqlConnectionInformation<sCN, dCN> : IConnectionInformation<sCN, dCN> {

		public sCN SourceConnection { get; set; }

		public dCN DestinationConnection { get; set; }

		public string SourceConnectionString { get; set; }

		public string DestinationConnectionString { get; set; }
	}

	public class FromSqlConnectionInformation<sCN, dCN> : IConnectionInformation<sCN, dCN> {

		public sCN SourceConnection { get; set; }

		public dCN DestinationConnection { get; set; }

		public string SourceConnectionString { get; set; }

		public string DestinationConnectionString { get; set; }
	}

	public class ToSqlConnectionInformation : IConnectionInformation {

		public string SourceConnectionString { get; set; }

		public string DestinationConnectionString { get; set; }
	}

	public class FromSqlConnectionInformation : IConnectionInformation {

		public string SourceConnectionString { get; set; }

		public string DestinationConnectionString { get; set; }
	}
}