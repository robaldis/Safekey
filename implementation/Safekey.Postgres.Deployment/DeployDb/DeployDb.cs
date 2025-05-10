using System.Security.Cryptography;
using Npgsql;
using Safekey.Postgres.Deployment.Configuration;

namespace Safekey.Postgres.Deployment.DeployDb;

public static class DeployDb
{

    public static void DeployDatabase(DbConfiguration configuration)
    {
        //TODO: Write a test that will generate teh checksum

        // Get migrations scripst from file
        var scripts = GetMigrationScripts();

        var fileName = scripts.Select(x => x.sqlFile.Split("/").Last());
        Console.WriteLine($"MigrationScripts:\n{string.Join("\n", fileName)}");

        var hasValues = scripts.Select(x => (expected: GetHashValueFromFile(x.hashFile), actual: ComputeHash(x.sqlFile)));

        if (hasValues.Any(x => x.expected != x.actual))
        {
            var invalidScripts = hasValues
                .Where(x => x.expected != x.actual)
                .Select(x => $"Expected: {x.expected} But got: {x.actual}");

            var message = string.Join("\n", invalidScripts);

            throw new InvalidDataException($"Scripts are not vaild\n{message}");
        }

        // Get the deployment hash version
        var hash = GetDeployments(configuration);

        // Filter out the ones that haven't been applied
        var notDeployed = scripts
            .Where(x => 
                    !hash .Contains(GetHashValueFromFile(x.hashFile)))
            .Select(x => x);

        // Return early if there is nothi(ng to apply
        if (!notDeployed.Any()) return;

        // Apply the migration scripts in order!
        ApplyMigrationScript(configuration, notDeployed);
    }

    private static IEnumerable<(string hashFile, string sqlFile)> GetMigrationScripts()
    {
        // Get the migration scripts and hash files in code
        var files = Directory.GetFiles("../Safekey.Postgres.Deployment/MigrationScripts");
        var hash = files.Where(file => file.EndsWith("sha256")).Select(x => x);

        var scripts = files.Where(file => file.EndsWith("sql"))
            .Select(x => (GetHash(version(x), hash), x));
        return scripts;
    }

    private static string version(string path)
    {
        return path.Split("/").Last().Substring(0, 5);
    }

    private static string GetHash(string version, IEnumerable<string> path)
    {
        return path
            .Where(x => x.Split("/").Last().StartsWith(version))
            .Select(x => x)
            .Single();

    }

    private static string ComputeHash(string filePath)
    {
        var strText = System.IO.File.ReadAllBytes(filePath);
        using var sha = SHA256.Create();
        byte[] hashValue = sha.ComputeHash(strText);

        return Convert.ToBase64String(hashValue);
        //return System.Text.Encoding.UTF8.GetString(hashValue);
    }

    private static IEnumerable<string> GetDeployments(
            DbConfiguration configuration)
    {
        try {
            var source = configuration.GetConnection();
            using var connection = source.OpenConnectionAsync().Result;

            return GetAllDeploymentHash(connection);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("relation \"deployment\" does not exist"))
                return Enumerable.Empty<string>();
            else throw;
        }
    }

    private static void ApplyMigrationScript(
            DbConfiguration configuration,
            IEnumerable<(string, string)> migrationScripts)
    {
        // Connection
        var source = configuration.GetConnection();
        // Get the sql from file
        foreach(var (hashFile, file) in migrationScripts)
        {
            Console.WriteLine($"Executing file {file}");
            using var connection = source.OpenConnectionAsync().Result;
            var cmd = new NpgsqlCommand("", connection);
            var result = cmd.ExecuteFile(file);

            string hash = GetHashValueFromFile(hashFile);
            UpdateDeployment(connection, hash);
        }
        // Apply
        // Add hash to deployment

    }

    private static string GetHashValueFromFile(string hashFile)
    {
        return File.ReadAllText(hashFile, System.Text.Encoding.UTF8).Trim();
    }

    private static int ExecuteFile(this NpgsqlCommand cmd, string filename)
    {
        string strText = System.IO.File.ReadAllText(filename, System.Text.Encoding.UTF8);
        cmd.CommandText = strText;
        return cmd.ExecuteNonQuery();
    }

    private static int UpdateDeployment(NpgsqlConnection connection, string hash)
    {
        var cmd = new NpgsqlCommand(DeploymentCommands.command, connection);
        cmd.Parameters.AddWithValue("@value", hash);
        return cmd.ExecuteNonQuery();
    }

    private static IEnumerable<string> GetAllDeploymentHash(NpgsqlConnection connection)
    {
        var allHashes = new List<string>();
        var cmd = new NpgsqlCommand(DeploymentCommands.get, connection);
        var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            var hash = reader.GetFieldValue<string>(0);
            allHashes.Add(hash);
        }
        return allHashes;
    }


}
