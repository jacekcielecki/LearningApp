using LearningApp.Dal.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.Dal.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<WsbLearnDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("azureSqlDb"),
                    x => x.MigrationsAssembly(typeof(WsbLearnDbContext).Assembly.FullName)));

            return services;
        }
    }
}
