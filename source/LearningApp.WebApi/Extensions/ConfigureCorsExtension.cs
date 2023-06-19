namespace LearningApp.WebApi.Extensions
{
    public static class ConfigureCorsExtension
    {
        public static IServiceCollection AddCors(this IServiceCollection services, string corsPolicyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: corsPolicyName, builder =>
                    builder.AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin());
            });

            return services;
        }
    }
}
