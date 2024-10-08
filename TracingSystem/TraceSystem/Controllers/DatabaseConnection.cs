using MySql.Data.MySqlClient;

public class DatabaseConnection
{
    string connectionString = "Server=localhost;Port=3306;Database=trace_db;User ID=root;Password=root;";

    public MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }
}