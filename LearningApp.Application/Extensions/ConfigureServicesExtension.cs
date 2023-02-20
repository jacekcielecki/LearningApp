using FluentValidation;
using LearningApp.Application.Settings;
using LearningApp.Application.Validators.User;
using LearningApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.Application.Extensions
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
