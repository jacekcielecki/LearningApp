using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Achievement;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class AchievementControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;

        public AchievementControllerTests(WebApplicationFactory<Program> factory)
        {
            factory = factory.WithWebHostBuilder(builder =>
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

            _client = factory.CreateClient();
            _databaseSeeder = new DatabaseSeeder(factory);
        }

        [Fact]
        public async Task GetAllAsync_WithExistingItems_ReturnsItems()
        {
            //act
            var response = await _client.GetAsync("/api/Achievement");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<List<AchievementDto>>().Should().BeOfType<List<AchievementDto>>();
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsStatusOk()
        {
            //arrange
            var itemToCreate = new CreateAchievementRequest
            {
                Name = "TestAchievementName",
                Description = "TestAchievementDescription",
            };

            //act
            var response = await _client.PostAsync("/api/Achievement", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<AchievementDto>().Should().BeOfType<AchievementDto>();
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var itemToDelete = new Achievement
            {
                Name = "TestAchievementName",
            };
            await _databaseSeeder.Seed(itemToDelete);

            //act
            var response = await _client.DeleteAsync($"/api/Achievement/{itemToDelete.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_WithItemToUpdate_ReturnsStatusOk()
        {
            //arrange
            var itemToUpdate = new Achievement
            {
                Name = "TestAchievementName",
                Description = "TestAchievementDescription",
            };
            var updatedItem = new UpdateAchievementRequest
            {
                Name = "UpdatedTestAchievementName",
                Description = "UpdatedTestAchievementDescription",
            };
            await _databaseSeeder.Seed(itemToUpdate);

            //act
            var response = await _client.PatchAsync($"api/Achievement/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<AchievementDto>().Should().BeOfType<AchievementDto>();
        }
    }
}
