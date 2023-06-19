using LearningApp.Application.Authorization;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Services;
using LearningApp.WebApi.Middleware;
using Microsoft.AspNetCore.Authorization;

namespace LearningApp.WebApi.Extensions
{
    public static class ConfigureWebApiServicesExtension
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserProgressService, UserProgressService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<ICategoryGroupService, CategoryGroupService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler>();

            return services;
        }
    }
}
