using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WSBLearn.Application.Settings;
using WSBLearn.Application.Validators.User;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, JwtAuthenticationSettings jwtSettings, AzureBlobStorageSettings blobSettings)
        {
            services.AddAutoMapper(typeof(ConfigureServicesExtension).Assembly);
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
            services.AddSingleton(jwtSettings);
            services.AddSingleton(blobSettings);
        }
    }
}
