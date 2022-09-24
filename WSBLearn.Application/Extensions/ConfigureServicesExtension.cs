using Microsoft.Extensions.DependencyInjection;

namespace WSBLearn.Application.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ConfigureServicesExtension).Assembly);
        }
    }
}
