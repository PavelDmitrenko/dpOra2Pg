using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace dpOra2Pg
{
    public class Transition
    {
        private static Stopwatch stopwatch;
        private static long _rowsProcessed;
        private readonly SettingsModel _settings;
        private readonly OraStructureReader _oracle;
        private readonly PGStructureReader _postres;
        private OraStructure _oraStructure;
        private PGStructure _pgCurrentStructure;

        #region ctor
        public Transition(SettingsModel settings)
        {
            _settings = settings;
            _oracle = new OraStructureReader(_settings);
            _postres = new PGStructureReader(_settings);
        }
        #endregion

        #region ExecuteTransition
        public async Task ExecuteTransition()
        {
            _oraStructure = await _oracle.GetStructure();
            _pgCurrentStructure = await _postres.GetStructure();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            _rowsProcessed = 0;

            DefaultTypeMap.MatchNamesWithUnderscores = true;

            PGStructure pgStructure = new PGStructure();
            pgStructure.MapFromOracle(_settings, _oraStructure);

            await using (NpgsqlConnection pgConnection = new NpgsqlConnection(_settings.Postgres.ConnectionString))
            {
                Console.WriteLine("Opening Postgres connection...");
                await pgConnection.OpenAsync();

                Console.WriteLine("Transferring structure and data...");
                await using OracleConnection oraConnection = new OracleConnection(_settings.Oracle.ConnectionString);
                await oraConnection.OpenAsync();

                await using (NpgsqlTransaction pgTransaction = await pgConnection.BeginTransactionAsync(IsolationLevel.ReadCommitted))
                {
                    foreach (PGTable table in pgStructure.Tables.OrderBy(x => x.TableName))
                        await CreateTable(oraConnection, pgConnection, pgStructure, table);

                    await CreateSequences(pgConnection, pgStructure);

                    Console.WriteLine("Commiting transaction...");
                    await pgTransaction.CommitAsync();
                }
            }

            Console.WriteLine("Done");
            Console.WriteLine($"Rows processed: {_rowsProcessed}");
            Console.WriteLine($"Elapsed: {stopwatch.Elapsed:mm\\:ss\\.ff}");

            stopwatch.Stop();
            stopwatch.Reset();
        }
        #endregion

        #region MyRegion
        public async Task CreateSequences(NpgsqlConnection dbPgConn, PGStructure pgStructure)
        {
            string ddlSequences = pgStructure.Sequences.DDL();
            if (!string.IsNullOrEmpty(ddlSequences))
            {
                Console.WriteLine("Applying Sequences...");
                await dbPgConn.ExecuteAsync(ddlSequences);
            }
        } 
        #endregion

        #region CreateTable
        public async Task CreateTable(OracleConnection dbOraConn, NpgsqlConnection dbPgConn, PGStructure ts, PGTable table)
        {
            PGTable newTable = ts.Tables.First(x => x.TableName.Equals(table.TableName, StringComparison.InvariantCultureIgnoreCase));
            PGTable existingTable = _pgCurrentStructure.Tables.FirstOrDefault(x => x.TableName == table.TableName);
            OraTable oracleTable = _oraStructure.Table.First(x => x.TableName.Equals(table.TableName, StringComparison.InvariantCultureIgnoreCase));
            List<OraTableColumn> oraColumns = _oraStructure.TableColumn.Where(x => x.TableName.Equals(oracleTable.TableName)).OrderBy(x => x.ColumnId).ToList();
            string oraColumnsStr = string.Join(',', oraColumns.Select(x => $"\"{x.ColumnName}\""));

            bool createStructure = false;

            if (existingTable == null) // TODO Additioanl equality checks
            {
                createStructure = true;
                string ddlColumns = newTable.DDLTable();
                await dbPgConn.ExecuteAsync(ddlColumns);
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{newTable.TableName.ToUpper()}");
            Console.ResetColor();

            int countTable = 0;
            int countConsoleStats = 0;
            string command = $"SELECT {oraColumnsStr} FROM \"{_settings.Oracle.Schema}\".\"{oracleTable.TableName}\"";
            //command = "select * from  ( SELECT \"GRN_RECORD_ID\",\"OGRNIP\",\"REG_DATE\",\"INN\",\"SURNAME\",\"NAME\",\"PATRONYMIC\",\"REGION_ID\",\"AREA_ID\",\"CITY_ID\",\"SETTLEMENT_ID\",\"TERM_DATE\" FROM \"EGRUL\".\"GRN_RECORD_ACTUAL_OGRNIP\") where ROWNUM <= 1000";

            long rowsAwaited = _oraStructure.Table.First(x => x.TableName.Equals(table.TableName, StringComparison.InvariantCultureIgnoreCase)).NumRows;

            await dbPgConn.ExecuteAsync($"DELETE FROM {_settings.Postgres.Schema}.\"{newTable.TableName}\"");

            await using (OracleCommand oraCommand = new OracleCommand(command, dbOraConn))
            {
                oraCommand.InitialLOBFetchSize = -1; // https://community.oracle.com/thread/4119841

                string binaryImportCommand = $"COPY {_settings.Postgres.Schema}.\"{newTable.TableName}\" ({newTable.ColumnsList()}) FROM STDIN (FORMAT BINARY)";

                await using (NpgsqlBinaryImporter pgWriter = dbPgConn.BeginBinaryImport(binaryImportCommand))
                {
                    using (IDataReader oraReader = await oraCommand.ExecuteReaderAsync(CommandBehavior.SingleResult))
                    {
                        while (oraReader.Read())
                        {
                            await pgWriter.StartRowAsync();

                            foreach (PGColumn column in newTable.Columns)
                                await pgWriter.WriteAsync(oraReader[column.OrdinalPosition - 1], column.DbType);

                            if (countConsoleStats == 1000)
                            {
                                decimal proc = Math.Round(countTable * 100 / (decimal)rowsAwaited, 2);
                                Utils.ConsoleWriteStats($"{countTable} ({proc}%)");
                                countConsoleStats = 0;
                            }

                            _rowsProcessed++;
                            countConsoleStats++;
                            countTable++;
                        }
                    }

                    await pgWriter.CompleteAsync();
                }
            }

            Utils.ConsoleWriteStats(countTable.ToString(), true);

            if (createStructure)
            {
                string ddlPK = newTable.PrimaryKey?.DDL();
                if (!string.IsNullOrEmpty(ddlPK))
                {
                    Console.WriteLine("Adding PrimaryKey...");
                    await dbPgConn.ExecuteAsync(ddlPK);
                }

                string ddlIndexes = newTable.Indexes.DDL();
                if (!string.IsNullOrEmpty(ddlIndexes))
                {
                    Console.WriteLine("Applying Indexes...");
                    await dbPgConn.ExecuteAsync(ddlIndexes);
                }

                string ddlUniqueKeys = newTable.UniqueKeys.DDL();
                if (!string.IsNullOrEmpty(ddlUniqueKeys))
                {
                    Console.WriteLine("Applying Unique Keys...");
                    await dbPgConn.ExecuteAsync(ddlUniqueKeys);
                }
            }

            Console.WriteLine("done");
            Console.WriteLine();
        }
        #endregion

    }
}
