using FluentValidation;
using LearningApp.Application.Requests.User;
using LearningApp.Application.Settings;
using LearningApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.Application.Extensions
{
    public static class ConfigureApplicationServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services, 
            JwtAuthenticationSettings jwtSettings,
            AzureBlobStorageSettings blobSettings, 
            SmtpSettings smtpSettings)
        {
            services.AddAutoMapper(typeof(ConfigureApplicationServicesExtension).Assembly);
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
            services.AddSingleton(jwtSettings);
            services.AddSingleton(blobSettings);
            services.AddSingleton(smtpSettings);
        }
    }
}
