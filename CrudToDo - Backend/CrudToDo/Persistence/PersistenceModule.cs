using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrudToDo.Persistence
{
    public class PersistenceModule
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TodoDatabase")));

            services.AddScoped<IPersistanceContext>((serviceProvider) =>
            {
                return new CorePersistenceContext(serviceProvider);
            });
        }
    }
}
