using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WSBLearn.Dal.Persistence;

namespace WSBLearn.Dal.Extensions
{
    public static class ApplyMigrationsExtension
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<WsbLearnDbContext>();
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
