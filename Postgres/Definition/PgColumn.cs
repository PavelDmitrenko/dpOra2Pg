using System;
using NpgsqlTypes;

namespace dpOra2Pg
{
	public class PgColumn
	{
		public string Name { get; set; }
		public string Comment { get; set; }
		public int OrderIndex { get; set; }
		public string Type { get; set; }
		public NpgsqlDbType DbType { get; set; }
	
	

		public PgColumn(OraTableColumn column, string columnComments)
		{
			Name = column.ColumnName.ToLower();
			
			(NpgsqlDbType dbType, string type) = ColumnTypeMap.ToPostgres(column);
			DbType = dbType;
			Type = type;

			OrderIndex = column.ColumnId;
			Comment = columnComments;

		}
	}
}
