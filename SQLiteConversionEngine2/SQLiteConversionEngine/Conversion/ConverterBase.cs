using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using SQLiteConversionEngine.Utility;

namespace SQLiteConversionEngine.Conversion {
	/// <summary>
	///
	/// </summary>
	/// <typeparam name="sCN">The type of the Source Connection.</typeparam>
	/// <typeparam name="sCM">The type of the Source Command.</typeparam>
	/// <typeparam name="dCN">The type of the Destination Connection.</typeparam>
	/// <typeparam name="dCM">The type of the Source Command.</typeparam>
	public abstract class ConverterBase<sCN, sCM, dCN, dCM> : IConverter<sCN, sCM, dCN, dCM> {
	}
}