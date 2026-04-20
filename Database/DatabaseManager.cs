using MySql.Data.MySqlClient;

namespace HealthTechClinic.Database
{
    public interface IDatabaseManager
    {
        MySqlConnection CreateOpenConnection();
    }

    public class DatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public MySqlConnection CreateOpenConnection()
        {
            var connection = new MySqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}