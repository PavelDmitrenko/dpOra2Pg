using System;
using NpgsqlTypes;

namespace dpOra2Pg
{
	public static class ColumnTypeMap
	{
        public static Tuple<NpgsqlDbType, string> ToPostgres(PGTableColumnBase column)
        {
            switch (column.udt_name.ToLower())
            {
                case "int8":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Bigint, $"int8");

                case "int4":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Smallint, $"int4");

				case "int2":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Smallint, $"int2");

                case "numeric":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Numeric, $"numeric({column.numeric_precision},{column.numeric_scale})");

				case "varchar":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Varchar, $"varchar({column.character_octet_length})");

                case "date":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Date, $"date");

				case "timestamp":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Timestamp, $"timestamp");

				case "bpchar":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Char, $"char({column.CharacterMaximumLength})");

                case "text":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Text, $"text");


                case "bytea":
                    return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Bytea, $"bytea");

				default:
                    throw new ArgumentOutOfRangeException(column.DataType);
            }
        }

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
				case "LONG":
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
                case "RAW":
					return new Tuple<NpgsqlDbType, string>(NpgsqlDbType.Bytea, $"bytea");

				default:
					throw new ArgumentOutOfRangeException(column.DataType);
			}
		}
	}
}
