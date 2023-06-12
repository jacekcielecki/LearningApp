using LearningApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.Infrastructure.Extensions
{
    public static class ConfigureInfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string sqlServerConnectionString)
        {
            services.AddDbContext<LearningAppDbContext>(
                options => options.UseSqlServer(sqlServerConnectionString,
                    x => x.MigrationsAssembly(typeof(LearningAppDbContext).Assembly.FullName)));

            return services;
        }
    }
}
