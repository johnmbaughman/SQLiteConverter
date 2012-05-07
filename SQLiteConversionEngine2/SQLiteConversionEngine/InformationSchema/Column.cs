namespace SQLiteConversionEngine.InformationSchema {
	public class Column : InformationSchemaBase {

		public string ColumnType { get; set; }

		public int Length { get; set; }

		public int Precision { get; set; }

		public bool IsNullable { get; set; }

		public string DefaultValue { get; set; }

		public bool IsIdentity { get; set; }

		public bool? IsCaseSensitive { get; set; }

		public bool IsPrimaryKey { get; set; }
	}
}