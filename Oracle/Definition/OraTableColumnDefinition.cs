
using System;

namespace dpOra2Pg
{
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
