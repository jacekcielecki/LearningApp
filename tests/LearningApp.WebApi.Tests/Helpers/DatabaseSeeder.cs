using LearningApp.Infrastructure.Persistence;

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

            await dbContext!.Set<T>().AddAsync(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
