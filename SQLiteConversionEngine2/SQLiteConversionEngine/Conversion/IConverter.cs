using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SQLiteConversionEngine.Conversion {
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="sCN">The type of the Source Connection.</typeparam>
	/// <typeparam name="sCM">The type of the Source Command.</typeparam>
	/// <typeparam name="dCN">The type of the Destination Connection.</typeparam>
	/// <typeparam name="dCM">The type of the Source Command.</typeparam>
	public interface IConverter<sCN, sCM, dCN, dCM> {
	}
}