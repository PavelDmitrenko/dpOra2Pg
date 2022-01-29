using System;
using NpgsqlTypes;

namespace dpOra2Pg
{
	public class PGColumn
	{
		public string Name { get; set; }
		public string Comment { get; set; }
		public int OrdinalPosition { get; set; }
		public string Type { get; set; }
		public NpgsqlDbType DbType { get; set; }
         
        public PGColumn(PGTableColumnBase baseColumn)
        {
            Name = baseColumn.ColumnName.ToLower();
            (NpgsqlDbType dbType, string type) = ColumnTypeMap.ToPostgres(baseColumn);

			//(NpgsqlDbType dbType, string type) = ColumnTypeMap.ToPostgres(column);
			//DbType = dbType;
			//Type = type;

			OrdinalPosition = baseColumn.OrdinalPosition;
            //Comment = baseColumn.com;
        }

		public PGColumn(OraTableColumn column, string columnComments)
		{
			Name = column.ColumnName.ToLower();
			
			(NpgsqlDbType dbType, string type) = ColumnTypeMap.ToPostgres(column);
			DbType = dbType;
			Type = type;

			OrdinalPosition = column.ColumnId;
			Comment = columnComments;

		}
	}
}
