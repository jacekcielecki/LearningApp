using LearningApp.Domain.Common;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.WebApi.Tests.Helpers
{
    public class DatabaseSeeder
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DatabaseSeeder(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public async Task Seed(object item)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            var dbContext = scope?.ServiceProvider.GetService<WsbLearnDbContext>();

            if (dbContext is null) return;

            switch (item)
            {
                case Achievement achievement:
                    await dbContext.Achievements.AddAsync(achievement);
                    break;
                case Category category:
                    await dbContext.Categories.AddAsync(category);
                    break;
                case CategoryGroup categoryGroup:
                    await dbContext.CategoryGroups.AddAsync(categoryGroup);
                    break;
                case CategoryProgress categoryProgress:
                    await dbContext.CategoryProgresses.AddAsync(categoryProgress);
                    break;
                case LevelProgress levelProgress:
                    await dbContext.LevelProgresses.AddAsync(levelProgress);
                    break;
                case Question question:
                    await dbContext.Questions.AddAsync(question);
                    break;
                case Role role:
                    await dbContext.Roles.AddAsync(role);
                    break;
                case User user:
                    await dbContext.Users.AddAsync(user);
                    break;
                case UserProgress userProgress:
                    await dbContext.UserProgresses.AddAsync(userProgress);
                    break;
                default:
                    throw new InvalidOperationException(Messages.DatabaseSeederFailure(item.GetType().ToString()));
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
