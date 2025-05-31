namespace ManagementAPI.Helpers
{
    using MySqlConnector;


    public class DatabaseInitializer
    {
        public void EnsureDatabaseExists(string connectionString)
        {
            var builder = new MySqlConnectionStringBuilder(connectionString);
            string databaseName = builder.Database;
            builder.Database = "";

            using var connection = new MySqlConnection(builder.ConnectionString);
            connection.Open();

            // Cria o banco se não existir
            using (var cmd = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS `{databaseName}`;", connection))
            {
                cmd.ExecuteNonQuery();
            }

            // Muda para o banco criado
            using (var cmd = new MySqlCommand($"USE `{databaseName}`;", connection))
            {
                cmd.ExecuteNonQuery();
            }

            // Cria a tabela se não existir
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS `user` (
                    `id` INT AUTO_INCREMENT NOT NULL,
                    `username` VARCHAR(255) NOT NULL,
                    `password` VARCHAR(255) NOT NULL,
                    `email` VARCHAR(255) NOT NULL,
                    `role` ENUM('admin', 'user') NOT NULL DEFAULT 'user',
                    `passwordRecovery` VARCHAR(255) NULL,
                    `otpCode` VARCHAR(255) NULL,
                    `otpExpiration` DATETIME NULL,
                    PRIMARY KEY(`id`)
                );
            ";

            using (var cmd = new MySqlCommand(createTableSql, connection))
            {
                cmd.ExecuteNonQuery();
            }
        }


    }
}
