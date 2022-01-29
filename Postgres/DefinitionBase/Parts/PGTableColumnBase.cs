
namespace dpOra2Pg
{
	public class PGTableColumnBase
	{
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public int OrdinalPosition { get; set; }
        public string ColumnDefault { get; set; }
        public string IsNullable { get; set; }
        public string DataType { get; set; }
        public int CharacterMaximumLength { get; set; }
        public int character_octet_length { get; set; }
        public int numeric_precision { get; set; }
        public int numeric_precision_radix { get; set; }
        public int numeric_scale { get; set; }
        public string datetime_precision { get; set; }
        public string interval_type { get; set; }
        public string interval_precision { get; set; }
        public string character_set_catalog { get; set; }
        public string character_set_schema { get; set; }
        public string character_set_name { get; set; }
        public string collation_catalog { get; set; }
        public string collation_schema { get; set; }
        public string collation_name { get; set; }
        public string domain_catalog { get; set; }
        public string domain_schema { get; set; }
        public string udt_catalog { get; set; }
        public string udt_schema { get; set; }
        public string udt_name { get; set; }
        public string scope_catalog { get; set; }
        public string scope_schema { get; set; }
        public string maximum_cardinality { get; set; }
        public long? dtd_identifier { get; set; }
        public string is_self_referencing { get; set; }
        public string is_identity { get; set; }
        public string identity_generation { get; set; }
        public long? identity_start { get; set; }
        public long? identity_increment { get; set; }
        public long? identity_maximum { get; set; }
        public long? identity_minimum { get; set; }
        public string identity_cycle { get; set; }
        public string is_generated { get; set; }
        public string generation_expression { get; set; }
        public string is_updatable { get; set; }

    }

}
