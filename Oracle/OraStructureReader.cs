using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace dpOra2Pg
{
    public class OraStructureReader
    {
        private readonly SettingsModel _settings;
 
        public OraStructureReader(SettingsModel settings)
        {
            _settings = settings;
        }

        public async Task<OraStructure> GetStructure()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            await using OracleConnection oraConnection = new OracleConnection(_settings.Oracle.ConnectionString);

            Console.WriteLine("Opening Oracle connection...");
            await oraConnection.OpenAsync();
            
            Console.WriteLine("Getting Oracle structure...");
            string sqlTable = Utils.ReadSqlFromResource("Table.sql", "Oracle");
            string sqlTableColumn = Utils.ReadSqlFromResource("TableColumn.sql", "Oracle");
            string sqlTableColumnComments = Utils.ReadSqlFromResource("TableColumnComment.sql", "Oracle");
            string sqlIndex = Utils.ReadSqlFromResource("Index.sql", "Oracle");
            string sqlIndexColumn = Utils.ReadSqlFromResource("IndexColumn.sql", "Oracle");
            string sqlConstraint = Utils.ReadSqlFromResource("Constraint.sql", "Oracle");
            string sqlConstraintColumn = Utils.ReadSqlFromResource("ConstraintColumn.sql", "Oracle");
            string sqlSequence = Utils.ReadSqlFromResource("Sequence.sql", "Oracle");
            
            OraStructure result = new OraStructure();

            _settings.Oracle.Schema = _settings.Oracle.Schema.ToUpper();

            // Constraints
            result.Constraint = oraConnection.Query<OraConstraint>(sqlConstraint, new { owner = _settings.Oracle.UserID }).ToList();
            result.ConstraintColumn = oraConnection.Query<OraConstraintColumn>(sqlConstraintColumn, new { owner = _settings.Oracle.UserID }).ToList();
     
            // Table
            result.Table = oraConnection.Query<OraTable>(sqlTable, new { owner = _settings.Oracle.Schema }).ToList();
            result.TableColumn = oraConnection.Query<OraTableColumn>(sqlTableColumn, new { owner = _settings.Oracle.Schema }).ToList();
            result.TableColumnComment = oraConnection.Query<OraTableColumnComment>(sqlTableColumnComments, new { owner = _settings.Oracle.Schema }).ToList();

            result.Index = oraConnection.Query<OraIndex>(sqlIndex, new { owner = _settings.Oracle.Schema }).ToList();
            result.IndexColumn = oraConnection.Query<OraIndexColumn>(sqlIndexColumn, new { owner = _settings.Oracle.Schema }).ToList();

            // Sequence
            result.Sequence = oraConnection.Query<OraSequence>(sqlSequence, new { owner = _settings.Oracle.Schema }).ToList();

            return result;
        }
    }
}
