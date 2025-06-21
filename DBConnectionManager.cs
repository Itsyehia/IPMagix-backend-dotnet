using Microsoft.Data.SqlClient;

public class DBConnectionManager
{
    private readonly IConfiguration _config;
    public DBConnectionManager(IConfiguration config) => _config = config;
    public SqlConnection GetConnection()
    {
        var connStr = _config.GetConnectionString("DefaultConnection");
        return new SqlConnection(connStr);
    }
} 