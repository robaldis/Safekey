
using Safekey.Postgres.Deployment.Configuration;
using Safekey.Postgres.Deployment.DeployDb;

namespace Safekey.Webapi;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var configuration = new DbConfiguration();
        DeployDb.DeployDatabase(configuration);
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
