namespace SQLiteConversionEngine.InformationSchema {
	public class Column : InformationSchemaBase {

		public string ColumnType { get; set; }

		public int Length { get; set; }

		public int Precision { get; set; }

		public bool IsNullable { get; set; }

		public string DefaultValue { get; set; }

		public bool IsIdentity { get; set; }

		public bool? IsCaseSensitive { get; set; }

		/// <summary>
		/// When defining an index, gets or sets a value indicating whether this instance is ascending.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is ascending; otherwise, <c>false</c>.
		/// </value>
		public bool IsAscending { get; set; }
	}
}