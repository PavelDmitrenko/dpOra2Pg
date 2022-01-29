using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpOra2Pg
{
	public class PGForeignKeys
	{
		public readonly List<PgForeignKey> ForeignKeys;

		private PGForeignKeys()
		{
			ForeignKeys = new List<PgForeignKey>();
		}

		public PGForeignKeys(List<OraConstraint> constraints, List<OraConstraintColumn> constraintColumns) : this()
		{
			List<OraConstraint> currentConstrains = constraints.Where(x =>  x.ConstraintType == OraConstraintType.ReferentialIntegrity).ToList();

			foreach (OraConstraint constraint in currentConstrains)
			{
				PgForeignKey pgForeignKey = new PgForeignKey(constraint, constraintColumns);
				ForeignKeys.Add(pgForeignKey);
			}
		}
	}


	public class PgForeignKey
	{
		public string Name { get; set; }
		public string TableName { get; set; }
		public List<PgConstraintColumn> Columns { get; set; }
		public PGDeferrable Deferrable { get; set; }
		public bool Enabled { get; set; }
		public string DeleteRule { get; set; }
		public bool Validated { get; set; }

		private PgForeignKey()
		{
			Columns = new List<PgConstraintColumn>();
			
		}

		public PgForeignKey(OraConstraint constraint, List<OraConstraintColumn> constraintColumns) : this()
		{
			Name = constraint.ConstraintName;
			TableName = constraint.TableName;
			Deferrable = new PGDeferrable(constraint.Deferrable, constraint.Deferred);
			Enabled = constraint.Status.Equals("ENABLED", StringComparison.InvariantCultureIgnoreCase);
			Validated = constraint.Validated.Equals("VALIDATED", StringComparison.InvariantCultureIgnoreCase);
			DeleteRule = constraint.DeleteRule;

			List<OraConstraintColumn> oraColumns = constraintColumns.Where(x => x.ConstraintName == constraint.ConstraintName).OrderBy(x => x.Position).ToList();
			if (oraColumns.Count != 1)
			{
			}

			foreach (OraConstraintColumn oraColumn in oraColumns)
			{
				PgConstraintColumn pgConstraintColumn = new PgConstraintColumn(oraColumn);
				Columns.Add(pgConstraintColumn);
			}
		}

	}

	public class PgConstraintColumn
	{
		public string ColumnName { get; set; }
		public int Position { get; set; }

		public PgConstraintColumn(OraConstraintColumn oraConstraintColumn)
		{
			ColumnName = oraConstraintColumn.ColumnName;
			Position = oraConstraintColumn.Position;
		}
	}
}
