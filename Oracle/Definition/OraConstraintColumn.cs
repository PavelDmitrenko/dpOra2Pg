using System;
using System.Collections.Generic;
using System.Text;

namespace dpOra2Pg
{
	public class OraConstraintColumn
	{
		public string Owner { get; set; }
		public string ConstraintName { get; set; }
		public string TableName { get; set; }
		public string ColumnName { get; set; }
		public int Position { get; set; }

	}
}
