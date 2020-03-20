using System.Collections.Generic;
using System.Linq;

namespace dpOra2Pg
{
	public class PgStructure
	{
		public List<PgTable> Tables;
		
		public void ToPostgresSchema(SettingsPostgres pgSettings,
									List<OraTable> tables, 
									List<OraTableColumn> columns, 
									List<OraTableColumnComment> columnComments,
									List<OraIndex> index,
									List<OraIndexColumn> indexColumn
		)
		{
			Tables = new List<PgTable>();
			ILookup<string, OraTableColumn> columnsLookup = columns.ToLookup(k => k.TableName, v => v);
			ILookup<string, OraTableColumnComment> columnsCommentsLookup = columnComments.ToLookup(k => k.TableName, v => v);
			ILookup<string, OraIndex> indexLookup = index.ToLookup(k => k.TableName, v => v);
			ILookup<string, OraIndexColumn> indexColumnLookup = indexColumn.ToLookup(k => k.TableName, v => v);

			foreach (OraTable table in tables.Where(x => !x.IsTemporary))
			{
				Tables.Add(new PgTable(pgSettings, table, columnsLookup, columnsCommentsLookup, indexLookup, indexColumnLookup));
			}
		}

	}
}
