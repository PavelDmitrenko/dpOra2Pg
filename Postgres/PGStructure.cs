using System;
using System.Collections.Generic;
using System.Linq;

namespace dpOra2Pg
{
    public class PGStructure
    {
        public List<PGTable> Tables { get; private set; }
        public PGSequences Sequences { get; private set; }
        public PGForeignKeys ForeignKeys { get; private set; }

        public void MapFromBaseStructure(string schemaName, PGBaseStructure baseStructure)
        {
            Tables = new List<PGTable>();
            foreach (PGTableBase baseTable in baseStructure.Tables.Where(x => x.TableSchema.Equals(schemaName, StringComparison.InvariantCultureIgnoreCase)))
            {
                List<PGTableColumnBase> columns = baseStructure.TablesColumns.Where(x => x.TableSchema == baseTable.TableSchema && x.TableName == baseTable.TableName).ToList();
                PGTable table = new PGTable(baseTable, columns);
                Tables.Add(table);
            }
        }

        public void MapFromOracle(SettingsModel settings, OraStructure oraStructure)
        {
            Tables = new List<PGTable>();
            ILookup<string, OraTableColumn> columnsLookup = oraStructure.TableColumn.ToLookup(k => k.TableName, v => v);
            ILookup<string, OraTableColumnComment> columnsCommentsLookup = oraStructure.TableColumnComment.ToLookup(k => k.TableName, v => v);
            ILookup<string, OraIndex> indexLookup = oraStructure.Index.ToLookup(k => k.TableName, v => v);
            ILookup<string, OraIndexColumn> indexColumnLookup = oraStructure.IndexColumn.ToLookup(k => k.TableName, v => v);
            ILookup<string, OraConstraint> constraintsLookup = oraStructure.Constraint.ToLookup(k => k.TableName, v => v);
            ILookup<string, OraConstraintColumn> constraintsColumnsLookup = oraStructure.ConstraintColumn.ToLookup(k => k.TableName, v => v);


            foreach (OraTable table in oraStructure.Table.Where(x => settings.Settings.CreateTempTables || !x.IsTemporary))//.Where(x =>x.TableName.Equals("_testz2", StringComparison.InvariantCultureIgnoreCase) ))
            {
                List<OraTableColumn> columns = columnsLookup[table.TableName].OrderBy(x => x.ColumnId).ToList();
                List<OraTableColumnComment> columnsComments = columnsCommentsLookup[table.TableName].ToList();
                List<OraIndex> indexes = indexLookup[table.TableName].ToList();
                List<OraIndexColumn> indexColumns = indexColumnLookup[table.TableName].ToList();
                List<OraConstraint> constraints = constraintsLookup[table.TableName].ToList();
                List<OraConstraintColumn> constraintsColumns = constraintsColumnsLookup[table.TableName].ToList();

                PGTable pgTable = new PGTable(schema: settings.Postgres.Schema, userId: settings.Postgres.UserID, constraintPrefix: settings.Postgres.ConstraintSuffix,
                                                table: table, columns: columns, columnComments: columnsComments, 
                                                indexes: indexes, indexColumns: indexColumns, constraints: constraints, 
                                                constraintsColumns: constraintsColumns);
                Tables.Add(pgTable);
            }

            Sequences = new PGSequences(settings.Postgres, oraStructure);
            ForeignKeys = new PGForeignKeys(oraStructure.Constraint, oraStructure.ConstraintColumn);
        }

    }
}
