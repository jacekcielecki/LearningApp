using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Category;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class CategoryControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;

        public CategoryControllerTests(WebApplicationFactory<Program> factory)
        {
            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<LearningAppDbContext>));

                    if (dbContextOptions is not null) services.Remove(dbContextOptions);

                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<LearningAppDbContext>(options => options.UseInMemoryDatabase("CategoryControllerTests"));
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
            response.Content.DeserializeHttpContent<List<CategoryDto>>().Should().BeOfType<List<CategoryDto>>();
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsItemById()
        {
            //arrange
            var existingItem = new Category
            {
                Name = "TestCategoryName",
            };
            await _databaseSeeder.Seed(existingItem);

            //act
            var response = await _client.GetAsync($"api/Category/{existingItem.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryDto>().Should().BeOfType<CategoryDto>();
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsStatusOk()
        {
            //arrange
            var itemToCreate = new CreateCategoryRequest
            {
                Name = "TestCategoryName",
                Description = "TestCategoryDescription",
                QuizPerLevel = 3,
                QuestionsPerQuiz = 5
            };

            //act
            var response = await _client.PostAsync("api/Category", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryDto>().Should().BeOfType<CategoryDto>();
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdateReturnsStatusOk()
        {
            //arrange
            var itemToUpdate = new Category
            {
                Name = "TestCategoryName",
                CreatorId = 1
            };
            await _databaseSeeder.Seed(itemToUpdate);

            var updatedItem = new UpdateCategoryRequest
            {
                Name = "UpdatedTestCategoryName",
                Description = "UpdatedTestCategoryDescription",
                QuizPerLevel = 3,
                QuestionsPerQuiz = 5
            };

            //act
            var response = await _client.PutAsync($"api/Category/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryDto>().Should().BeOfType<CategoryDto>();
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var itemToDelete = new Category
            {
                Name = "TestCategoryName",
                CreatorId = 1
            };
            await _databaseSeeder.Seed(itemToDelete);

            //act
            var response = await _client.DeleteAsync($"api/Category/{itemToDelete.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
