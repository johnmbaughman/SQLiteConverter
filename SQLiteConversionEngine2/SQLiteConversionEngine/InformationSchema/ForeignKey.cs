namespace SQLiteConversionEngine.InformationSchema {
	public class ForeignKey : InformationSchemaBase {

		/// <summary>
		/// Gets or sets the foreign key table.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The foreign key table.</value>
		public string ForeignKeyTable { get; set; }

		/// <summary>
		/// Gets or sets From (local) column.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>From column.</value>
		public string FromColumn { get; set; }

		/// <summary>
		/// Gets or sets To (foreign) column.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>To column.</value>
		public string ToColumn { get; set; }

		/// <summary>
		/// Gets or sets the On Update action.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The on update.</value>
		public string OnUpdate { get; set; }

		/// <summary>
		/// Gets or sets the On Delete action.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The on delete.</value>
		public string OnDelete { get; set; }

		/// <summary>
		/// Gets or sets the Match action.
		/// (Derived from SQLite)
		/// </summary>
		/// <value>The match.</value>
		public string Match { get; set; }
	}
}