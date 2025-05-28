namespace ManagementAPI.Settings
{
    public class DbConfig
    {
        public required string ConnectionString { get; init; }

        public static DbConfig FromEnvironment()
        {
            return new DbConfig
            {
                ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                    ?? throw new Exception("DB_CONNECTION_STRING não configurada.")
            };
        }
    }
}