namespace dpOra2Pg
{
	public class OraSequence
	{
		public string SequenceName { get; set; }
		public long MinValue { get; set; }
		public decimal MaxValue { get; set; }
		public long IncrementBy { get; set; }
		public string CycleFlag { get; set; }
		public string OrderFlag { get; set; }
		public long CacheSize { get; set; }
		public long LastNumber { get; set; }
	
	}
}
