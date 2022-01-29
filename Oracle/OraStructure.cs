using System.Collections.Generic;

namespace dpOra2Pg
{
    public class OraStructure
    {
        public List<OraConstraint> Constraint;
        public List<OraConstraintColumn> ConstraintColumn;
        public List<OraTable> Table;
        public List<OraTableColumn> TableColumn;
        public List<OraTableColumnComment> TableColumnComment;
        public List<OraIndex> Index;
        public List<OraIndexColumn> IndexColumn;
        public List<OraSequence> Sequence;
    }
}