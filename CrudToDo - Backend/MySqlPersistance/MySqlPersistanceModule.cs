using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MySqlPersistance
{
    public class MySqlPersistanceModule
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<MySqlDbContext>(_ => new MySqlDbContext(configuration.GetConnectionString("MySqlDatabase")));

            services.AddScoped<IPersistanceContext>((serviceProvider) =>
            {
                return new MySqlPersistanceContext(serviceProvider);
            });
        }
    }
}
