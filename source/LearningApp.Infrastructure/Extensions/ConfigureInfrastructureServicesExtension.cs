using LearningApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.Infrastructure.Extensions
{
    public static class ConfigureInfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string sqlServerConnectionString)
        {
            services.AddDbContext<WsbLearnDbContext>(
                options => options.UseSqlServer(sqlServerConnectionString,
                    x => x.MigrationsAssembly(typeof(WsbLearnDbContext).Assembly.FullName)));

            return services;
        }
    }
}
