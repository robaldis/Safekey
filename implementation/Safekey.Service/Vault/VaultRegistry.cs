
using Npgsql;
using Safekey.Postgres.Deployment.Configuration;

namespace Safekey.Service.Vault;

public class VaultRegistry : IVaultRegistry
{
    private readonly DbConfiguration _configuration;

    public VaultRegistry(
            DbConfiguration configuration
            )
    {
        _configuration = configuration;
    }

    public void CreateSecret(string key, byte[] secret, byte[] IV)
    {
        var source = _configuration.GetConnection();
        using var connection = source.OpenConnection();
        var sql = VaultSqlCommands.CreateKey();
        var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters
            .AddWithValue("@key", key);
        cmd.Parameters
            .AddWithValue("@value", secret);
        cmd.Parameters
            .AddWithValue("@iv", IV);
        cmd.ExecuteNonQuery();
    }


    public Secret GetSecret(string key)
    {
        var source = _configuration.GetConnection();
        using var connection = source.OpenConnection();
        var sql = VaultSqlCommands.GetKey();
        var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters
            .AddWithValue("@key", key);
        var reader = cmd.ExecuteReader();
        byte[] value = new byte[1];
        byte[] iv = new byte[1];
        while (reader.Read())
        {
            value = reader.GetFieldValue<byte[]>(1);
            iv = reader.GetFieldValue<byte[]>(2);

        }
        return new Secret(key, value, iv);
    }

    public void DeleteSecret(string key)
    {
        var source = _configuration.GetConnection();
        using var connection = source.OpenConnection();
        var sql = VaultSqlCommands.DeleteKey();
        var cmd = new NpgsqlCommand(sql, connection);
        cmd.Parameters
            .AddWithValue("@key", key);
        cmd.ExecuteNonQuery();
    }
}



