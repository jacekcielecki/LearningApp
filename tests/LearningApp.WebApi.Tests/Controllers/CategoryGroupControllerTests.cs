using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.CategoryGroup;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class CategoryGroupControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;

        public CategoryGroupControllerTests()
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
        public async Task GetAllAsync_WithExistingItems_ReturnsStatusOk()
        {
            //act
            var response = await _client.GetAsync("api/CategoryGroup");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<List<CategoryGroupDto>>().Should().BeOfType<List<CategoryGroupDto>>();
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsItemById()
        {
            //arrange
            var existingItem = new CategoryGroup
            {
                Name = "TestCategoryGroupName"
            };
            await _databaseSeeder.Seed(existingItem);

            //act
            var response = await _client.GetAsync($"api/CategoryGroup/{existingItem.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryGroupDto>().Should().BeOfType<CategoryGroupDto>();
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsStatusOk()
        {
            //arrange
            var itemToCreate = new CreateCategoryGroupRequest
            {
                Name = "TestCategoryGroupName",
            };

            //act
            var response = await _client.PostAsync("api/CategoryGroup", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryGroupDto>().Should().BeOfType<CategoryGroupDto>();
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdate_ReturnsStatusOk()
        {
            //arrange
            var itemToUpdate = new CategoryGroup
            {
                Name = "TestCategoryGroupName"
            };
            await _databaseSeeder.Seed(itemToUpdate);

            var updatedItem = new UpdateCategoryGroupRequest
            {
                Name = "UpdatedTestCategoryGroupName"
            };

            //act
            var response = 
                await _client.PutAsync($"api/CategoryGroup/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryGroupDto>().Should().BeOfType<CategoryGroupDto>();
        }

        [Fact]
        public async Task AddCategory_WithValidCategoryAndGroupIds_AddCategoryToGroupById()
        {
            //arrange
            var existingCategoryGroup = new CategoryGroup
            {
                Name = "TestCategoryGroupName"
            };
            var existingCategory = new Category
            {
                Name = "TestCategoryName",
                LessonsPerLevel = 5,
                QuestionsPerLesson = 5
            };
            await _databaseSeeder.Seed(existingCategoryGroup);
            await _databaseSeeder.Seed(existingCategory);

            //act
            var response =
                await _client.PutAsync($"api/CategoryGroup/addCategory/{existingCategoryGroup.Id}/{existingCategory.Id}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryGroupDto>().Should().BeOfType<CategoryGroupDto>();
        }

        [Fact]
        public async Task RemoveCategory_WithValidCategoryAndGroupIds_AddCategoryToGroupById()
        {
            //arrange
            var existingCategoryGroup = new CategoryGroup
            {
                Name = "TestCategoryGroupName"
            };
            var existingCategory = new Category
            {
                Name = "TestCategoryName",
                LessonsPerLevel = 5,
                QuestionsPerLesson = 5
            };
            await _databaseSeeder.Seed(existingCategoryGroup);
            await _databaseSeeder.Seed(existingCategory);

            //act
            var response =
                await _client.PutAsync($"api/CategoryGroup/addCategory/{existingCategoryGroup.Id}/{existingCategory.Id}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<CategoryGroupDto>().Should().BeOfType<CategoryGroupDto>();
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var itemToDelete = new CategoryGroup
            {
                Name = "TestCategoryGroupName"
            };
            await _databaseSeeder.Seed(itemToDelete);
            
            //act
            var response = await _client.DeleteAsync($"api/CategoryGroup/{itemToDelete.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
