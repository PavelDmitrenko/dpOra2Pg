using System;
using System.Collections.Generic;
using System.Linq;

namespace dpOra2Pg
{
    public class PGPrimaryKey
    {
        public string Name { get; }
        public string TableName { get; }
        public PGDeferrable Deferrable { get; }
        public bool Enabled { get; }
        public string DeleteRule { get; }
        public bool Validated { get; }
        public string IndexName { get; }
        public List<string> Columns { get; }
        private readonly string _schema;
        private readonly string _tableName;
        private readonly string _constraintSuffix;

        #region ctor
        private PGPrimaryKey()
        {
            Columns = new List<string>();
        }

        public PGPrimaryKey(string tableName, string schema, OraConstraint constraint, List<OraConstraintColumn> constraintColumns, string constraintSuffix) : this()
        {
            _schema = schema;
            _tableName = tableName;
            _constraintSuffix = constraintSuffix;

            Name = constraint.ConstraintName.ToLower();
            TableName = constraint.TableName.ToLower();
            Deferrable = new PGDeferrable(constraint.Deferrable, constraint.Deferred);
            Enabled = constraint.Status.Equals("ENABLED", StringComparison.InvariantCultureIgnoreCase);
            Validated = constraint.Validated.Equals("VALIDATED", StringComparison.InvariantCultureIgnoreCase);
            DeleteRule = constraint.DeleteRule;
            IndexName = constraint.IndexName.ToLower();

            List<OraConstraintColumn> pkColumns = constraintColumns.Where(x => x.ConstraintName.Equals(constraint.ConstraintName, StringComparison.InvariantCultureIgnoreCase)).ToList();
            foreach (OraConstraintColumn pkColumn in pkColumns.OrderBy(x => x.Position))
                Columns.Add(pkColumn.ColumnName.ToLower());
        }
        #endregion

        #region DDL
        public string DDL()
        {
            string sqlNotNull = string.Empty;
            foreach (string column in Columns)
                sqlNotNull += $"ALTER COLUMN {column} SET NOT NULL,\n";

            string columns = string.Join(',', Columns);
            
            string sql = $"ALTER TABLE {_schema}.{_tableName}\n" +
                         $"{sqlNotNull}" +
                         $"ADD CONSTRAINT {Name}{_constraintSuffix} PRIMARY KEY({columns}) USING INDEX TABLESPACE \"pg_default\" {Deferrable.Deferrable};";

            return sql;
        } 
        #endregion

    }

}