namespace SQLiteConversionEngine.InformationSchema {
	public class Index : InformationSchemaBase {

		/// <summary>
		/// When defining an index, gets or sets a value indicating whether this instance is ascending.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is ascending; otherwise, <c>false</c>.
		/// </value>
		public bool IsAscending { get; set; }
	}
}