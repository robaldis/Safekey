
using Npgsql;

namespace Safekey.Postgres.Deployment.Configuration;

public class DbConfiguration
{
    private readonly string _host;
    private readonly string _port;
    private readonly string _database;
    private readonly string _user;
    private readonly string _password;

    public DbConfiguration()
    {
        //Get the configuration from env vars
        var database = Environment.GetEnvironmentVariable("database");
        var host = Environment.GetEnvironmentVariable("host");
        var port = Environment.GetEnvironmentVariable("port");
        var user = Environment.GetEnvironmentVariable("user");
        var password = Environment.GetEnvironmentVariable("adminpassword");

        _host = "localhost";
        _port = "5432";
        _database = "safekey";
        _user = "admin";
        _password = "adminpassword";
    }

    public NpgsqlDataSource GetConnection()
    {
        var connectionString = $"Host={_host}:{_port};Username={_user};Password={_password};Database={_database}";
        return NpgsqlDataSource.Create(connectionString);
    }
}
