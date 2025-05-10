

namespace Safekey.Postgres.Deployment.DeployDb;

public static class DeploymentCommands
{
    public static string command => @"
        INSERT INTO deployment (hash) 
        VALUES (@value);";

    public static string get => @"
        Select hash from deployment;";
}
