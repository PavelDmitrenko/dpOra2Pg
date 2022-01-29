using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace dpOra2Pg
{
    public class PGStructureReader
    {
        private readonly SettingsModel _settings;
 
        public PGStructureReader(SettingsModel settings)
        {
            _settings = settings;
        }

        public async Task<PGStructure> GetStructure()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            Console.WriteLine("Opening PostgreSQL connection...");
            await using NpgsqlConnection pgConnection = new NpgsqlConnection(_settings.Postgres.ConnectionString);
            await pgConnection.OpenAsync();

            Console.WriteLine("Getting PostgreSQL structure...");
            string sqlTable = Utils.ReadSqlFromResource("Table.sql", "Postgres");
            string sqlTableColumn = Utils.ReadSqlFromResource("TableColumn.sql", "Postgres");

        
            PGBaseStructure baseStructure = new PGBaseStructure();

            _settings.Oracle.Schema = _settings.Oracle.Schema.ToUpper();

            // Constraints
            //result.Constraint = oraConnection.Query<OraConstraint>(sqlConstraint, new { owner = _settings.Oracle.UserID }).ToList();
            //result.ConstraintColumn = oraConnection.Query<OraConstraintColumn>(sqlConstraintColumn, new { owner = _settings.Oracle.UserID }).ToList();

            // Table
            baseStructure.Tables = pgConnection.Query<PGTableBase>(sqlTable, new { owner = _settings.Oracle.Schema }).ToList();
            baseStructure.TablesColumns = pgConnection.Query<PGTableColumnBase>(sqlTableColumn, new { owner = _settings.Oracle.Schema }).ToList();
            //result.TableColumnComment = oraConnection.Query<OraTableColumnComment>(sqlTableColumnComments, new { owner = _settings.Oracle.Schema }).ToList();

            //result.Index = oraConnection.Query<OraIndex>(sqlIndex, new { owner = _settings.Oracle.Schema }).ToList();
            //result.IndexColumn = oraConnection.Query<OraIndexColumn>(sqlIndexColumn, new { owner = _settings.Oracle.Schema }).ToList();

            // Sequence
            //result.Sequence = oraConnection.Query<OraSequence>(sqlSequence, new { owner = _settings.Oracle.Schema }).ToList();
            
            PGStructure result = new PGStructure();
            result.MapFromBaseStructure(_settings.Postgres.Schema, baseStructure);

            return result;
        }
    }
}
