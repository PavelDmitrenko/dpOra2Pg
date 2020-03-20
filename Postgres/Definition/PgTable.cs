using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpOra2Pg
{
	public class PgTable
	{
		public string TableName { get; }
		public List<PgColumn> Columns { get; }
		public List<PgIndex> Indexes { get; }
		private readonly SettingsPostgres _pgSettings;

		private PgTable(SettingsPostgres pgSettings)
		{
			Columns = new List<PgColumn>();
			Indexes = new List<PgIndex>();
			_pgSettings = pgSettings;
		}

		public PgTable(SettingsPostgres pgSettings,
						OraTable table,
						ILookup<string, OraTableColumn> columns,
						ILookup<string, OraTableColumnComment> columnComments,
						ILookup<string, OraIndex> indexLookup,
						ILookup<string, OraIndexColumn> indexColumnLookup) : this(pgSettings)
		{
			TableName = table.TableName.ToLower();
		
			foreach (OraTableColumn column in columns[table.TableName].OrderBy(x => x.ColumnId))
			{
				string comment = columnComments[table.TableName].FirstOrDefault(x => x.ColumnName == column.ColumnName)?.Comments;
				Columns.Add(new PgColumn(column, comment));
			}

			foreach (OraIndex oraIndex in indexLookup[table.TableName])
			{
				Indexes.Add(new PgIndex(oraIndex, indexColumnLookup[table.TableName].ToList()));
			}
		}

		public string OraColumnsList()
		{
			return string.Join(',', Columns.Select(x => $"\"{x.Name.ToUpper()}\""));
		}
		
		public string ColumnsList()
		{
			return string.Join(',', Columns.Select(x => x.Name));
		}

		public string DDLColumns()
		{
			StringBuilder sbTable = new StringBuilder();
			StringBuilder sbColumns = new StringBuilder();

			foreach (PgColumn column in Columns)
				sbColumns.AppendFormat("{0} {1}, ", column.Name, column.Type);

			string ddlColumns = sbColumns.ToString().Trim(' ', ',');

			string drop = $"DROP TABLE IF EXISTS {_pgSettings.Schema}.{TableName};";
			string create = $"CREATE TABLE {_pgSettings.Schema}.{TableName}({ddlColumns});";
			string setOwner = $"ALTER TABLE {_pgSettings.Schema}.{TableName} OWNER to {_pgSettings.UserID};";

			sbTable.Append(drop);
			sbTable.Append(create);
			sbTable.Append(setOwner);

			return sbTable.ToString();
		}

		public string DDLIndexes()
		{
			StringBuilder sbIndexes = new StringBuilder();
			foreach (PgIndex index in Indexes)
			{
				string unique = index.IsUnique ? "UNIQUE" : null;
				sbIndexes.AppendLine($"CREATE {unique} INDEX {index.Name}");
				sbIndexes.AppendLine($"ON {_pgSettings.Schema}.{TableName} USING btree");
				sbIndexes.Append("(");

				StringBuilder sbColumns = new StringBuilder();
				foreach (PgIndexColumn column in index.Columns.OrderBy(x => x.ColumnIndex))
					sbColumns.Append($"{column.ColumnName} {column.SortOrder} NULLS LAST,");
				
				sbIndexes.Append(sbColumns.ToString().Trim(','));
				sbIndexes.Append(")");
				sbIndexes.AppendLine("TABLESPACE pg_default;");
			}
			
			return sbIndexes.ToString();

		}
	}

}
