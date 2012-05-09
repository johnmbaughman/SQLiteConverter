namespace SQLiteConversionEngine.InformationSchema.SqlServer {
	public class ColumnCollection : InformationSchemaCollectionBase<Column> {

		public override Column Find(string name) {
			return this.Find(t => t.ColumnName == name);
		}
	}
}