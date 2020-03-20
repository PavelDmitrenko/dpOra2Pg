using System;
using NpgsqlTypes;

namespace dpOra2Pg
{
	public static class ColumnTypeMap
	{
		public static Tuple<NpgsqlDbType, string> ToPostgres(OraTableColumn column)
		{
			switch (column.DataType.ToUpper())
			{
				case "NUMBER" when column.DataScale == 0 && column.DataPrecision == 0:
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Numeric, "numeric");

				case "NUMBER" when column.DataScale == 0 && column.DataPrecision <= 4:
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Smallint, "smallint");

				case "NUMBER" when column.DataScale == 0 && column.DataPrecision > 4  && column.DataPrecision <= 9:
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Integer, "int");

				case "NUMBER" when column.DataScale == 0 && column.DataPrecision > 9  && column.DataPrecision <= 18:
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Bigint, "bigint");

				case "NUMBER" when column.DataScale == 0 && column.DataPrecision > 18:
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Numeric, $"numeric({column.DataPrecision})");

				case "VARCHAR2":
				case "NVARCHAR2":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Varchar, $"varchar({column.CharLength})");

				case "CHAR":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Char, $"char({column.CharLength})");

				case "TIMESTAMP(6)":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Timestamp, $"timestamp(6)");
				
				case "DATE":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Timestamp, $"timestamp(0)");

				case "CLOB":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Text, $"text");

				case "BLOB":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Bytea, $"bytea");

				default:
					throw new ArgumentOutOfRangeException(column.DataType);
			}
		}
	}
}
