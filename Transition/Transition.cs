using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace dpOra2Pg
{
	public class Transition
	{

		private static bool _counterInitialized;
		private static Stopwatch stopwatch;
		private static long _rowsProcessed;
		private readonly SettingsModel _settings;

		public Transition(SettingsModel settings)
		{
			_settings = settings;
		}

		public async Task GetOraStructure()
		{

			stopwatch = new Stopwatch();
			stopwatch.Start();
			_rowsProcessed = 0;

			DefaultTypeMap.MatchNamesWithUnderscores = true;
			PgStructure ts = new PgStructure();
			
			using (OracleConnection oraConnection = new OracleConnection(_settings.Oracle.ConnectionString))
			{
				Console.Write("Opening Oracle connection...");
				await oraConnection.OpenAsync();
				Console.WriteLine("done");

				Console.Write("Getting Oracle structure...");
				string sqlTable = _ReadSqlFromResource("Table.sql");
				string sqlTableColumn = _ReadSqlFromResource("TableColumn.sql");
				string sqlTableColumnComments = _ReadSqlFromResource("TableColumnComment.sql");
				string indexSql = _ReadSqlFromResource("Index.sql");
				string indexColumnSql = _ReadSqlFromResource("IndexColumn.sql");

				var tableDefinitions = oraConnection.Query<OraTable>(sqlTable, new { owner = _settings.Oracle.UserID }).ToList();
				var tableColumnDefinitions = oraConnection.Query<OraTableColumn>(sqlTableColumn, new { owner = _settings.Oracle.UserID }).ToList();
				var tableColumnCommentDefinitions = oraConnection.Query<OraTableColumnComment>(sqlTableColumnComments, new { owner = _settings.Oracle.UserID }).ToList();
				List<OraIndex> indexDefinition = oraConnection.Query<OraIndex>(indexSql, new { owner = _settings.Oracle.UserID }).ToList();
				List<OraIndexColumn> indexColumnDefinition = oraConnection.Query<OraIndexColumn>(indexColumnSql, new { owner = _settings.Oracle.UserID }).ToList();
				
				Console.WriteLine("done");

				ts.ToPostgresSchema(_settings.Postgres, tableDefinitions, tableColumnDefinitions, tableColumnCommentDefinitions, indexDefinition, indexColumnDefinition);
				
				using (NpgsqlConnection pgConnection = new NpgsqlConnection(_settings.Postgres.ConnectionString))
				{
					Console.Write("Opening Postgres connection...");
					await pgConnection.OpenAsync();
					Console.WriteLine("done");

					Console.WriteLine("Transferring structure and data...");
					using (NpgsqlTransaction pgTransaction = pgConnection.BeginTransaction(IsolationLevel.ReadCommitted))
					{
						foreach (PgTable table in ts.Tables)
						{
							await AddTable(oraConnection, pgConnection, ts, table.TableName);
						}

						Console.Write("Commiting transaction...");
						await pgTransaction.CommitAsync();
						Console.WriteLine("done");
					}
				}
			}

			Console.WriteLine("Done.");
			Console.WriteLine($"Rows processed: {_rowsProcessed}");
			Console.WriteLine($"Elapsed: {stopwatch.Elapsed:mm\\:ss\\.ff}");

			stopwatch.Stop();
			stopwatch.Reset();

		}

		public async Task AddTable(OracleConnection dbOraConn, NpgsqlConnection dbPgConn, PgStructure ts, string tableName)
		{
			PgTable tableData = ts.Tables.First(x => x.TableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));

			string ddlColumns = tableData.DDLColumns();
			dbPgConn.Execute(ddlColumns);

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"{tableData.TableName.ToUpper()}");
			Console.ResetColor();

			int countTable = 0;
			int countConsoleStats = 0;

			string command = $"SELECT {tableData.OraColumnsList()} FROM {tableData.TableName}";

			using (OracleCommand oraCommand = new OracleCommand(command, dbOraConn))
			{
				oraCommand.InitialLOBFetchSize = -1; // https://community.oracle.com/thread/4119841

				using (NpgsqlBinaryImporter pgWriter = dbPgConn.BeginBinaryImport($"COPY {_settings.Postgres.Schema}.{tableData.TableName} ({tableData.ColumnsList()}) FROM STDIN (FORMAT BINARY)"))
				{
					using (IDataReader oraReader = await oraCommand.ExecuteReaderAsync(CommandBehavior.SingleResult))
					{
						while (oraReader.Read())
						{
							pgWriter.StartRow();

							foreach (PgColumn column in tableData.Columns)
								await pgWriter.WriteAsync(oraReader[column.OrderIndex - 1], column.DbType);

							if (countConsoleStats == 1000)
							{
								_ConsoleWriteStats(countTable);
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

			_ConsoleWriteStats(countTable, true);

			Console.Write("Applying indexes...");
			string ddlIndexes = tableData.DDLIndexes();
			dbPgConn.Execute(ddlIndexes);
			Console.WriteLine("done");
			Console.WriteLine();
		}

		private static string _ReadSqlFromResource(string name)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string resourcePath = name;

			if (!name.StartsWith(nameof(dpOra2Pg)))
				resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(name));

			using Stream stream = assembly.GetManifestResourceStream(resourcePath);
			using StreamReader reader = new StreamReader(stream);
			return reader.ReadToEnd().Replace("@", ":");
		}

		private static void _ConsoleWriteStats(int c, bool final = false)
		{
			if (_counterInitialized) // Dont erase on first iteration
				Console.Write(new string('\b', (c - 1).ToString().Length));
			else
			{
				_counterInitialized = true;
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					Console.CursorVisible = false;
			}

			Console.Write(c);

			if (final)
			{
				Console.WriteLine();
				_counterInitialized = false;
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					Console.CursorVisible = true;
			}

			Console.ResetColor();
		}

	}
}
