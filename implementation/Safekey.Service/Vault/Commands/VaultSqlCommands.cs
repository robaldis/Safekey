
public static class VaultSqlCommands
{

    public static string CreateKey() => @"INSERT INTO vault (key, secret, iv) VALUES (@key, @value, @iv)";

    public static string DeleteKey() => @"DELETE FROM vault WHERE key = @key;";

    public static string GetKey() => @"SELECT key, secret, iv from vault WHERE key = @key";
}
