using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dpOra2Pg
{
    public class PGTable
    {
        public string TableName { get; }
        public List<PGColumn> Columns { get; }
        public PGIndexes Indexes { get; }
        public PGPrimaryKey PrimaryKey { get; }
        public PGUniqueKeys UniqueKeys { get;  }

        private readonly string _schema;
        private readonly string _userId;
        private PGTable(string schema, string userId)
        {
            Columns = new List<PGColumn>();
            _schema = schema;
            _userId = userId;
        }

        public PGTable(PGTableBase baseTable, List<PGTableColumnBase> columns) 
        {
            TableName = baseTable.TableName;
            Columns = new List<PGColumn>();

            foreach (PGTableColumnBase baseColumn in columns)
            {
                PGColumn column = new PGColumn(baseColumn);
                Columns.Add(column);
            }
        }

        public PGTable(string schema, 
                        string userId,
                        string constraintPrefix,
                        OraTable table,
                        List<OraTableColumn> columns,
                        List<OraTableColumnComment> columnComments,
                        List<OraIndex> indexes,
                        List<OraIndexColumn> indexColumns,
                        List<OraConstraint> constraints,
                        List<OraConstraintColumn> constraintsColumns) : this(schema, userId)
        {
            TableName = table.TableName.ToLower();

            foreach (OraTableColumn column in columns)
            {
                string comment = columnComments.FirstOrDefault(x => x.ColumnName == column.ColumnName)?.Comments;
                Columns.Add(new PGColumn(column, comment));
            }

            // PK
            OraConstraint pkConstraint = constraints.FirstOrDefault(x => x.IndexOwner?.ToUpper() == schema.ToUpper() && x.ConstraintType == OraConstraintType.PrimaryKey);
            if (pkConstraint != null)
                PrimaryKey = new PGPrimaryKey(TableName, _schema, pkConstraint, constraintsColumns, constraintPrefix);

            // Indexes
            Indexes = new PGIndexes(TableName, _schema, indexes, indexColumns, constraints, constraintPrefix);

            // UniqueKeys
            UniqueKeys = new PGUniqueKeys(TableName, _schema, constraints, constraintsColumns, constraintPrefix);
        }

        public string ColumnsList()
        {
            return string.Join(',', Columns.Select(x => x.Name));
        }

        #region DDLTable
        public string DDLTable()
        {
            StringBuilder sbTable = new StringBuilder();
            StringBuilder sbColumns = new StringBuilder();

            foreach (PGColumn column in Columns)
            {
                // TODO Reserved List
                if (column.Name.Equals("session_user", StringComparison.InvariantCultureIgnoreCase))
                    column.Name += "_1";

                sbColumns.AppendFormat("{0} {1}, ", column.Name, column.Type);
            }

            string ddlColumns = sbColumns.ToString().Trim(' ', ',');

            string drop = $"DROP TABLE IF EXISTS {_schema}.{TableName};";
            string create = $"CREATE TABLE {_schema}.{TableName}({ddlColumns});";
            string setOwner = $"ALTER TABLE {_schema}.{TableName} OWNER to {_userId};";

            sbTable.Append(drop);
            sbTable.Append(create);
            sbTable.Append(setOwner);

            return sbTable.ToString();
        }
        #endregion

    }

}
