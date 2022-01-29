using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpOra2Pg
{
    public class PGUniqueKeys
    {
        private readonly string _schema;
        private readonly List<PGUniqueKey> _pgUniqueKeys;
        private readonly string _tableName;
        private readonly string _constraintSuffix;

        #region ctor
        public PGUniqueKeys(string tableName, string schema, List<OraConstraint> constraint, List<OraConstraintColumn> constraintColumns, string constraintSuffix)
        {
            _schema = schema;
            _pgUniqueKeys = new List<PGUniqueKey>();
            _tableName = tableName;
            _constraintSuffix = constraintSuffix;

            foreach (OraConstraint oraUniqueKey in constraint.Where(x => x.ConstraintType == OraConstraintType.UniqueKey && x.IndexOwner.Equals(_schema, StringComparison.InvariantCultureIgnoreCase)))
            {
                List<OraConstraintColumn> columns = constraintColumns.Where(x => x.ConstraintName.Equals(oraUniqueKey.ConstraintName)).ToList();
                PGUniqueKey pgUniqueKey = new PGUniqueKey(oraUniqueKey, columns);
                _pgUniqueKeys.Add(pgUniqueKey);
            }
        }
        #endregion

        #region DDL
        public string DDL()
        {
            StringBuilder sb = new StringBuilder();
            foreach (PGUniqueKey uniqueKey in _pgUniqueKeys)
            {
                string columns = string.Join(',', uniqueKey.Columns);

                string sql = $"ALTER TABLE {_schema}.{_tableName}\n" +
                             $"ADD CONSTRAINT {uniqueKey.UniqueKeyName}{_constraintSuffix} UNIQUE ({columns});";

                sb.AppendLine(sql);
            }
            return sb.ToString();
        }
        #endregion
    }

    public class PGUniqueKey
    {
        public string UniqueKeyName { get; set; }
        public List<string> Columns { get; set; }

        #region ctor
        public PGUniqueKey(OraConstraint oraSequence, List<OraConstraintColumn> constraintColumns)
        {
            UniqueKeyName = oraSequence.ConstraintName.ToLower();
            Columns = new List<string>();

            foreach (OraConstraintColumn column in constraintColumns.OrderBy(x => x.Position))
                Columns.Add(column.ColumnName.ToLower());

        }
        #endregion

    }

}