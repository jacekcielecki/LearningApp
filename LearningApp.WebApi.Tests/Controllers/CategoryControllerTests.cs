using LearningApp.Application.Requests.Category;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;

        public CategoryControllerTests()
        {
            var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
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
            var response = await _client.GetAsync("api/Category");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsItemById()
        {
            //arrange
            var existingItem = new Category
            {
                Id = 1,
                Name = "TestCategory 1",
            };
            await _databaseSeeder.Seed(existingItem);

            //act
            var response = await _client.GetAsync($"api/Category/{existingItem.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsStatusOk()
        {
            //arrange
            var itemToCreate = new CreateCategoryRequest
            {
                Name = "TestCategory 2",
                Description = "TestCategory Description 2",
                LessonsPerLevel = 3,
                QuestionsPerLesson = 5
            };

            //act
            var response = await _client.PostAsync("api/Category", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdateReturnsStatusOk()
        {
            //arrange
            var existingItem = new Category
            {
                Id = 2,
                Name = "TestCategory 3",
            };
            await _databaseSeeder.Seed(existingItem);

            var updatedItem = new UpdateCategoryRequest
            {
                Name = "Updated TestCategory 3",
                Description = "TestCategory Description 3",
                LessonsPerLevel = 3,
                QuestionsPerLesson = 5
            };

            //act
            var response = await _client.PutAsync($"api/Category/{existingItem.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var existingItem = new Category
            {
                Id = 2,
                Name = "TestCategory 4",
            };
            await _databaseSeeder.Seed(existingItem);

            //act
            var response = await _client.DeleteAsync($"api/Category/{existingItem.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
