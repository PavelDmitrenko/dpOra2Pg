namespace dpOra2Pg
{
	public class OraIndexColumn
	{
		public string IndexOwner { get; set; }
		public string IndexName { get; set; }
		public string TableOwner { get; set; }
		public string TableName { get; set; }
		public string ColumnName { get; set; }
		public int ColumnPosition { get; set; }
		public int ColumnLength { get; set; }
		public int CharLength { get; set; }
		public string Descend { get; set; }
	}
}
