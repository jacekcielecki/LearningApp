using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WSBLearn.Application.Requests;
using WSBLearn.Application.Validators;
using WSBLearn.Domain.Entities;

namespace WSBLearn.Application.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ConfigureServicesExtension).Assembly);
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();
        }
        //public static void RegisterValidators(this IServiceCollection services)
        //{
        //    services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        //}
    }
}
