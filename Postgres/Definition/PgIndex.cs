using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dpOra2Pg;

namespace dpOra2Pg
{

	[DebuggerDisplay("{Name}; Columns: {Columns.Count}; IsUnique: {IsUnique}")]
	public class PgIndex
	{
		public string Name { get; set; }
		public List<PgIndexColumn> Columns { get; set; }
		public bool IsUnique { get; set; }

		private PgIndex()
		{
			Columns = new List<PgIndexColumn>();
		}

		public PgIndex(OraIndex oraIndex, List<OraIndexColumn> oraIndexColumns) : this()
		{
			Name = oraIndex.IndexName;
			IsUnique = oraIndex.Uniqueness.Equals("UNIQUE", StringComparison.InvariantCultureIgnoreCase);

			List<OraIndexColumn> oraColumnIndexes = oraIndexColumns
				.Where(x => x.IndexName.Equals(oraIndex.IndexName, StringComparison.InvariantCultureIgnoreCase))
					.OrderBy(x => x.ColumnPosition).ToList();

			Columns.AddRange(oraColumnIndexes.Select(x => new PgIndexColumn(x)));

		}
	}
}

[DebuggerDisplay("{ColumnName}; {ColumnIndex}: {SortOrder}")]
public class PgIndexColumn
{
	public string ColumnName { get; set; }
	public int ColumnIndex { get; set; }
	public string SortOrder { get; set; }

	public PgIndexColumn(OraIndexColumn oraIndexColumn)
	{
		ColumnName = oraIndexColumn.ColumnName;
		ColumnIndex = oraIndexColumn.ColumnPosition;
		SortOrder = oraIndexColumn.Descend;
	}
}
