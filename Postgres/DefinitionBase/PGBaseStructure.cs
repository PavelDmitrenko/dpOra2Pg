using System;
using System.Collections.Generic;
using System.Linq;

namespace dpOra2Pg
{
    public class PGBaseStructure
    {
        public List<PGTableBase> Tables { get; set; }
        public List<PGTableColumnBase> TablesColumns { get; set; }
    }
}
