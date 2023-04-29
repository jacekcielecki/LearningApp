using LearningApp.Application.Requests.CategoryGroup;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class CategoryGroupControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public CategoryGroupControllerTests()
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
        public async Task GetAllAsync_WithExistingItems_ReturnsStatusOk()
        {
            //act
            var response = await _client.GetAsync("api/CategoryGroup");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsItemById()
        {
            //arrange
            var existingItem = new CategoryGroup
            {
                Id = 1,
                Name = "TestCategoryGroup 1"
            };
            await CategoryGroupSeedDb(existingItem);

            //act
            var response = await _client.GetAsync($"api/CategoryGroup/{existingItem.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsStatusOk()
        {
            //arrange
            var itemToCreate = new CreateCategoryGroupRequest
            {
                Name = "TestCategoryGroup 2",
            };

            //act
            var response = await _client.PostAsync("api/CategoryGroup", itemToCreate.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdate_ReturnsStatusOk()
        {
            //arrange
            var itemToUpdate = new CategoryGroup
            {
                Id = 3,
                Name = "TestCategoryGroup 3"
            };
            await CategoryGroupSeedDb(itemToUpdate);

            var updatedItem = new UpdateCategoryGroupRequest
            {
                Name = "Updated TestCategoryGroup 3"
            };

            //act
            var response = 
                await _client.PutAsync($"api/CategoryGroup/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddCategory_WithValidCategoryAndGroupIds_AddCategoryToGroupById()
        {
            //arrange
            var existingCategoryGroup = new CategoryGroup
            {
                Id = 4,
                Name = "Test CategoryGroup 4"
            };
            var existingCategory = new Category
            {
                Id = 5,
                Name = "Test Category 5",
                LessonsPerLevel = 5,
                QuestionsPerLesson = 5
            };
            await CategoryGroupSeedDb(existingCategoryGroup);
            await CategorySeedDb(existingCategory);

            //act
            var response =
                await _client.PutAsync($"api/CategoryGroup/addCategory/{existingCategoryGroup.Id}/{existingCategory.Id}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RemoveCategory_WithValidCategoryAndGroupIds_AddCategoryToGroupById()
        {
            //arrange
            var existingCategoryGroup = new CategoryGroup
            {
                Id = 5,
                Name = "Test CategoryGroup 5"
            };
            var existingCategory = new Category
            {
                Id = 6,
                Name = "Test Category 6",
                LessonsPerLevel = 5,
                QuestionsPerLesson = 5
            };
            await CategoryGroupSeedDb(existingCategoryGroup);
            await CategorySeedDb(existingCategory);

            //act
            var response =
                await _client.PutAsync($"api/CategoryGroup/addCategory/{existingCategoryGroup.Id}/{existingCategory.Id}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var existingItem = new CategoryGroup
            {
                Id = 6,
                Name = "Test CategoryGroup 6"
            };
            await CategoryGroupSeedDb(existingItem);

            //act
            var response = await _client.DeleteAsync($"api/CategoryGroup/{existingItem.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task CategoryGroupSeedDb(CategoryGroup item)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            var dbContext = scope?.ServiceProvider.GetService<WsbLearnDbContext>();

            if (dbContext != null)
            {
                await dbContext.CategoryGroups.AddAsync(item);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task CategorySeedDb(Category item)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory?.CreateScope();
            var dbContext = scope?.ServiceProvider.GetService<WsbLearnDbContext>();

            if (dbContext != null)
            {
                await dbContext.Categories.AddAsync(item);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
