using Safekey.Service.Vault;
using SimpleInjector;

namespace Safekey.Webapi
{
    public class Startup
    {
        private readonly Container container = new();
        public IConfiguration Configuration { get; }
        private readonly string applicationName = "safekey";
        private readonly string BasePath = "/safekey";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                .AddControllerActivation();
                container.Register<VaultCatalogue, VaultCatalogue>(Lifestyle.Singleton);
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSimpleInjector(container);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Setup middleware
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

