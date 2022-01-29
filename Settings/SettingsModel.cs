namespace dpOra2Pg
{
    public class SettingsModel
    {
        public SettingsOracle Oracle { get; set; }
        public SettingsPostgres Postgres { get; set; }
        public SettingsBase Settings { get; set; }
    }

    public class SettingsBase
    {
        public bool CreateTempTables { get; set; }
    }

    public class SettingsOracle
    {
        public string ConnectionString => $"DATA SOURCE={Host}:{Port}/{Service};PASSWORD={Password};USER ID={UserID};PERSIST SECURITY INFO=True";
        public string Host { get; set; }
        public int Port { get; set; }
        public string Service { get; set; }
        public string Schema { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
    public class SettingsPostgres
    {
        public string ConnectionString => $"Host={Host};Port={Port};Database={Database};User ID={UserID};Password={Password};Pooling=true;SSL Mode={SSLMode};Trust Server Certificate={TrustServerCertificate}";
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string SSLMode { get; set; }
        public bool TrustServerCertificate { get; set; }
        public string Schema { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string ConstraintSuffix { get; set; }
    }
}
