using LearningApp.Application.Interfaces;
using LearningApp.Application.Services;

namespace LearningApp.WebApi.Extensions
{
    public static class RegisterServicesExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IImageService, ImageService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserProgressService, UserProgressService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAchievementService, AchievementService>();
            services.AddScoped<ICategoryGroupService, CategoryGroupService>();

            return services;
        }
    }
}
