using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Helpers
{
    public class DatabaseSeeder
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DatabaseSeeder(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public async Task Seed<T>(T item) where T : class
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            var dbContext = scope?.ServiceProvider.GetService<LearningAppDbContext>();
            if (dbContext is null) return;

            if (item is User)
            {
                User user = (User) Convert.ChangeType(item, typeof(User));
                var existingUser = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                if (existingUser != null) return;
            }

            await dbContext.Set<T>().AddAsync(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
