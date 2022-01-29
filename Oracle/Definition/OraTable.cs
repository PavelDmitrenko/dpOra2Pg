
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
        public long NumRows { get; set; }
		public bool IsTemporary
		{
			get => Temporary == "Y";
			set => Temporary = value ? "Y" : "N";
		}
	}


}
