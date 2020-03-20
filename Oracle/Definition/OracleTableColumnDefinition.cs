
using System;

namespace dpOra2Pg
{

	public class OraTable
	{
		public string TableName { get; set; }
		public string TablespaceName { get; set; }
		public string ClusterName { get; set; }
		public string IOTName { get; set; }
		public long INITrans { get; set; }
		public long MAXTrans { get; set; }
		public string Status { get; set; }
		public string BackedUp { get; set; }
		public long PCTIncrease { get; set; }
		public long MAXExtents { get; set; }
		public long InitialExtent { get; set; }
		private string Temporary { get; set; }
		public string TableType { get; set; }
		public string TableTypeOwner { get; set; }
		public string Partitioned { get; set; }
		public DateTime? LastAnalyzed { get; set; }
		public long SampleSize { get; set; }
		public string TableLock { get; set; }
		public string Cache { get; set; }
		public string Instances { get; set; }
		public long AvgSpaceFreelistBlocks { get; set; }
		public long AVGRowLen { get; set; }
		public string AVGSpace { get; set; }
		public string SegmetCreated { get; set; }
		public string Duration { get; set; }
		public string UserStats { get; set; }
		public string GlobalStats { get; set; }
		public string CellFlashCache { get; set; }
		public string FlashCache { get; set; }
		public string Secondary { get; set; }
		public bool IsTemporary
		{
			get => Temporary == "Y";
			set => Temporary = value ? "Y" : "N";
		}
	}

	public class OraTableColumn
	{
		public string TableName { get; set; }
		public string ColumnName { get; set; }
		public int ColumnId { get; set; }
		public string DataType { get; set; }
		public string DataTypeMod { get; set; }
		public string DataTypeOwner { get; set; }
		public int DataLength { get; set; }
		public int DataPrecision { get; set; }
		public int DataScale { get; set; }
		private string Nullable { get; set; }
		public string DefaultLength { get; set; }
		public string DataDefault { get; set; }
		public byte[] LowValue { get; set; }
		public byte[] HighValue { get; set; }
		public DateTime? LastAnalyzed { get; set; }
		public int SampleSize { get; set; }
		public string CharacterSetName { get; set; }
		public int CharColDeclLength { get; set; }
		public string GlobalStats { get; set; }
		public string UserStats { get; set; }
		public int AvgColLen { get; set; }
		public int CharLength { get; set; }
		public string CharUsed { get; set; }
		public string V80FmtImage { get; set; }
		public string DataUpgraded { get; set; }
		public string Histogram { get; set; }

		public bool IsNullable
		{
			get => Nullable == "Y";
			set => Nullable = value ? "Y" : "N";
		}
	}

	public class OraTableColumnComment
	{
		public string TableName { get; set; }
		public string ColumnName { get; set; }
		public string Comments { get; set; }
	}
}
