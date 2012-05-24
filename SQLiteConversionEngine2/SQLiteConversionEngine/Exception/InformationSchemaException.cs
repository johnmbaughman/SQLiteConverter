using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SQLiteConversionEngine.Exception {
	public class InformationSchemaException<K, T> : System.Exception {
		private const string exceptionMessage = "SQLiteConversionEngine.Exception.InformationSchemaException";

		private string message = string.Empty;

		public InformationSchemaException(K collectionKey, T item, System.Exception innerException)
			: base(exceptionMessage, innerException) {
			message = string.Format("Key: {0}, Item type: {0}", collectionKey, item.ToString());
		}

		public override string Message {
			get {
				return message;
			}
		}
	}
}