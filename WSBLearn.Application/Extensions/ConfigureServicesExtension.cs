using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WSBLearn.Application.Validators;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(ConfigureServicesExtension).Assembly);
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

            var authenticationSettings = new JwtAuthenticationSettings();
            configuration.GetSection("Authentiaction").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);
        }
    }
}
