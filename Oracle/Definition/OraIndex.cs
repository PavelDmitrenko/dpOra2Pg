using System;

namespace dpOra2Pg
{
	public class OraIndex
	{	
		public string Owner { get; set; }
		public string IndexName { get; set; }
		public string IndexType { get; set; }
		public string TableOwner { get; set; }
		public string TableName { get; set; }
		public string TableType { get; set; }
		public string Uniqueness { get; set; }
		public string Compression { get; set; }
		public int PrefixLength { get; set; }
		public string TablespaceName { get; set; }
		public long IniTrans { get; set; }
		public long MaxTrans { get; set; }
		public long InitialExtent { get; set; }
		public long NextExtent { get; set; }
		public long MinExtents { get; set; }
		public long MaxExtents { get; set; }
		public long PctIncrease { get; set; }
		public long PctThreshold { get; set; }
		public long IncludeColumn { get; set; }
		public long Freelists { get; set; }
		public long FreelistGroups { get; set; }
		public string PctFree { get; set; }
		public string Logging { get; set; }
		public long Blevel { get; set; }
		public long LeafBlocks { get; set; }
		public long DistinctKeys { get; set; }
		public long AvgLeafBlocksPerKey { get; set; }
		public long AvgDataBlocksPerKey { get; set; }
		public long ClusteringFactor { get; set; }
		public string Status { get; set; }
		public long NumRows { get; set; }
		public long SampleSize { get; set; }
		public DateTime? LastAnalyzed { get; set; }
		public string Degree { get; set; }
		public string Instances { get; set; }
		public string Partitioned { get; set; }
		public string Temporary { get; set; }
		public string Generated { get; set; }
		public string Secondary { get; set; }
		public string BufferPool { get; set; }
		public string FlashCache { get; set; }
		public string CellFlashCache { get; set; }
		public string UserStats { get; set; }
		public string Duration { get; set; }
		public long PctDirectAccess { get; set; }
		public string ItypOwner { get; set; }
		public string ItypName { get; set; }
		public string Parameters { get; set; }
		public string GlobalStats { get; set; }
		public string DomidxStatus { get; set; }
		public string DomidxOpstatus { get; set; }
		public string FuncidxStatus { get; set; }
		public string JoinIndex { get; set; }
		public string IotRedundantPkeyElim { get; set; }
		public string Dropped { get; set; }
		public string Visibility { get; set; }
		public string DomidxManagement { get; set; }
		public string SegmentCreated { get; set; }
	}
}
