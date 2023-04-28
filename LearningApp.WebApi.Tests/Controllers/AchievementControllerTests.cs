using LearningApp.Application.Requests.Achievement;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class AchievementControllerTests
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public AchievementControllerTests()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<WsbLearnDbContext>));

                    if (dbContextOptions is not null) services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<WsbLearnDbContext>(options => options.UseInMemoryDatabase("InMemoryDb"));
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAllAsync_WithExistingItems_ReturnsItems()
        {
            //act
            var response = await _client.GetAsync("/api/Achievement");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsStatusOk()
        {
            //arrange
            var itemToCreate = new CreateAchievementRequest
            {
                Name = "Test Achievement Name",
                Description = "Test Achievement Description",
            };

            //act
            var response = await _client.PostAsync("/api/Achievement", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var itemToDelete = new Achievement
            {
                Id = 2,
                Name = "Test Achievement Name 2",
                Description = "Test Achievement Description 2",
            };
            await SeedDb(itemToDelete);

            //act
            var response = await _client.DeleteAsync("/api/Achievement/2");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_WithItemToUpdate_ReturnsStatusOk()
        {
            //arrange
            var itemToUpdate = new Achievement
            {
                Id = 3,
                Name = "Test Achievement Name 3",
                Description = "Test Achievement Description 3",
            };
            await SeedDb(itemToUpdate);

            var updatedItem = itemToUpdate;
            updatedItem.Description = "Updated description";

            //act
            var response = await _client.PatchAsync($"api/Achievement/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task SeedDb(Achievement item)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            var dbContext = scope?.ServiceProvider.GetService<WsbLearnDbContext>();

            await dbContext.Achievements.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }
    }
}
