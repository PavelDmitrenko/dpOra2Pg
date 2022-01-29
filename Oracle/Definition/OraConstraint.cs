using System;
using System.ComponentModel;

namespace dpOra2Pg
{
	public class OraConstraint
	{
		public string ConstraintName { get; set; }
		private string constraintType { get; set; }
		public string TableName { get; set; }
		public string SearchCondition { get; set; }
		public string ROwner { get; set; }
		public string RConstraintName { get; set; }
		public string DeleteRule { get; set; }
		public string Status { get; set; }
		public string Deferrable { get; set; }
		public string Deferred { get; set; }
		public string Validated { get; set; }
		public string Generated { get; set; }
		public string Bad { get; set; }
		public string Rely { get; set; }
		public string LastChange { get; set; }
		public string IndexOwner { get; set; }
		public string IndexName { get; set; }
		public string Invalid { get; set; }
		public string ViewRelated { get; set; }

		public OraConstraintType ConstraintType =>
			constraintType switch
			{
				"C" => OraConstraintType.CheckConstraintOnTable,
				"P" => OraConstraintType.PrimaryKey,
				"U" => OraConstraintType.UniqueKey,
				"R" => OraConstraintType.ReferentialIntegrity,
				"V" => OraConstraintType.WithСheckOptionOnView,
				"O" => OraConstraintType.WithReadOnlyOnView,
				"H" => OraConstraintType.HashExpression,
				"F" => OraConstraintType.ConstraintThatInvolvesREFColumn,
				"S" => OraConstraintType.SupplementalLogging,
				_ => throw new ArgumentOutOfRangeException()
			};
	}

	public enum OraConstraintType
	{
		[Description("C - Check constraint on a table")]
		CheckConstraintOnTable,

		[Description("P - Primary key")]
		PrimaryKey,

		[Description("U - Unique key")]
		UniqueKey,

		[Description("R - Referential integrity")]
		ReferentialIntegrity,

		[Description("V - With check option, on a view")]
		WithСheckOptionOnView,

		[Description("O - With read only, on a view")]
		WithReadOnlyOnView,

		[Description("H - Hash expression")]
		HashExpression,

		[Description("F - Constraint that involves a REF column")]
		ConstraintThatInvolvesREFColumn,

		[Description("S - Supplemental logging")]
		SupplementalLogging
	}

}
