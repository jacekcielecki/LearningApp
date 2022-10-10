using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ConfigureServicesExtension).Assembly);
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        }
    }
}
