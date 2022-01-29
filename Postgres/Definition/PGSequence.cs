using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpOra2Pg
{
    public class PGSequences
    {
        private readonly SettingsPostgres _pgSettings;
        private readonly List<PGSequence> _pgSequences;
        private readonly OraStructure _oraStructure;

        public PGSequences(SettingsPostgres pgSettings, OraStructure oraStructure)
        {
            _pgSettings = pgSettings;
            _oraStructure = oraStructure;
            _pgSequences = new List<PGSequence>();

            foreach (OraSequence oraSequence in oraStructure.Sequence)
            {
                PGSequence pgSequence = new PGSequence(oraSequence);
                _pgSequences.Add(pgSequence);
            }
        }

        #region DDL
        public string DDL()
        {
            List<string> tableNames = _oraStructure.Table.Select(x => x.TableName).ToList();
            //tableNames.AddRange(_oraStructure.Sequence.Select(x => x.SequenceName).ToList());

            StringBuilder sb = new StringBuilder();
            foreach (PGSequence pgSequence in _pgSequences)
            {
                while (tableNames.Exists(x => x.Equals(pgSequence.SequenceName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    pgSequence.SequenceName += "_seq";
                    List<string> sequenceNames = _oraStructure.Sequence.Select(x => x.SequenceName).ToList();

                    while (sequenceNames.Exists(x => x.Equals(pgSequence.SequenceName, StringComparison.InvariantCultureIgnoreCase)))
                        pgSequence.SequenceName += "_seq";
                }



                sb.AppendLine($"DROP SEQUENCE IF EXISTS {_pgSettings.Schema}.{pgSequence.SequenceName};");

                sb.AppendLine($"CREATE SEQUENCE {_pgSettings.Schema}.{pgSequence.SequenceName}");
                sb.AppendLine($"INCREMENT {pgSequence.Increment}");
                sb.AppendLine($"START {pgSequence.Start}");
                sb.AppendLine($"MINVALUE {pgSequence.MinValue}");

                if (pgSequence.MaxValue.HasValue)
                    sb.AppendLine($"MAXVALUE {pgSequence.MaxValue}");

                if (pgSequence.CacheSize.HasValue)
                    sb.AppendLine($"CACHE {pgSequence.CacheSize}");

                sb.AppendLine(";");
            }

            return sb.ToString();
        }
        #endregion
    }

    public class PGSequence
    {
        public string SequenceName { get; set; }
        public long Increment { get; set; }
        public long Start { get; set; }
        public long MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public long? CacheSize { get; set; }

        #region ctor
        public PGSequence(OraSequence oraSequence)
        {
            MaxValue = oraSequence.MaxValue >= 9223372036854775807 ? (decimal?)null : oraSequence.MaxValue;
            CacheSize = oraSequence.CacheSize == 0 ? (long?)null : oraSequence.CacheSize;

            SequenceName = oraSequence.SequenceName.ToLower();
            Increment = oraSequence.IncrementBy;
            Start = oraSequence.LastNumber;
            MinValue = oraSequence.MinValue;
        }
        #endregion

    }

}