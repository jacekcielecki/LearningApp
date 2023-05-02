using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Achievement;
using LearningApp.Application.Requests.CategoryGroup;
using LearningApp.Application.Services;
using LearningApp.Application.Tests.Helpers;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;

namespace LearningApp.Application.Tests.Services
{
    public class CategoryGroupServiceTests
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IValidator<CreateCategoryGroupRequest> _createCategoryGroupValidator = new CreateCategoryGroupValidator();
        private readonly IValidator<UpdateCategoryGroupRequest> _updateCategoryGroupValidator = new UpdateCategoryGroupValidator();

        public CategoryGroupServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<WsbLearnDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new WsbLearnDbContext(dbContextOptions);
        }

        [Fact]
        public async Task GetAllAsync_WithExistingItems_ReturnsAllItems()
        {
            //arrange
            var existingItems = new List<CategoryGroup>()
            {
                new() {Name = "TestCategoryGroup"},
                new() {Name = "TestCategoryGroup"},
                new() {Name = "TestCategoryGroup"}
            };
            await _dbContext.CategoryGroups.AddRangeAsync(existingItems);
            await _dbContext.SaveChangesAsync();


            var service = new CategoryGroupService(_dbContext, AutoMapperSingleton.Mapper,
                _createCategoryGroupValidator, _updateCategoryGroupValidator);

            //act
            var result = await service.GetAllAsync();

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<CategoryGroupDto>>();
            result.Should().ContainEquivalentOf(existingItems.FirstOrDefault(), options => options.ComparingByMembers<AchievementDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingItem_ReturnsItemById()
        {
            //arrange
            var existingItem = new CategoryGroup { Name = "TestCategoryGroup" };
            await _dbContext.CategoryGroups.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var service = new CategoryGroupService(_dbContext, AutoMapperSingleton.Mapper,
                _createCategoryGroupValidator, _updateCategoryGroupValidator);

            //act
            var result = await service.GetByIdAsync(existingItem.Id);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryGroupDto>();
            result.Should().BeEquivalentTo(existingItem,
                options => options.ComparingByMembers<CategoryGroupDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsCreatedItem()
        {
            //arrange
            var itemToCreate = new CreateCategoryGroupRequest { Name = "TestCategoryGroupName" };

            var service = new CategoryGroupService(_dbContext, AutoMapperSingleton.Mapper,
                _createCategoryGroupValidator, _updateCategoryGroupValidator);

            //act
            var result = await service.CreateAsync(itemToCreate);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryGroupDto>();
            result.Should().BeEquivalentTo(itemToCreate,
                options => options.ComparingByMembers<CategoryGroupDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdate_ReturnsUpdatedItem()
        {
            //arrange
            var existingItem = new CategoryGroup { Name = "TestCategoryGroup" };
            await _dbContext.CategoryGroups.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var updatedItem = new UpdateCategoryGroupRequest { Name = "UpdatedCategoryGroupName" };
            var service = new CategoryGroupService(_dbContext, AutoMapperSingleton.Mapper,
                _createCategoryGroupValidator, _updateCategoryGroupValidator);

            //act
            var result = await service.UpdateAsync(existingItem.Id, updatedItem);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryGroupDto>();
            result.Should().BeEquivalentTo(updatedItem,
                options => options.ComparingByMembers<CategoryGroupDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task AddCategoryAsync_WithValidCategoryGroupIdAndCategoryId_AddCategoryToGroup()
        {
            //arrange
            var existingCategory = new Category { Name = "TestCategory" };
            var existingCategoryGroup = new CategoryGroup { Name = "TestCategoryGroup" };
            await _dbContext.Categories.AddAsync(existingCategory);
            await _dbContext.CategoryGroups.AddAsync(existingCategoryGroup);
            await _dbContext.SaveChangesAsync();

            var service = new CategoryGroupService(_dbContext, AutoMapperSingleton.Mapper,
                _createCategoryGroupValidator, _updateCategoryGroupValidator);

            //act
            var result = await service.AddCategoryAsync(existingCategoryGroup.Id, existingCategory.Id);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CategoryGroupDto>();
            result.Name.Should().BeEquivalentTo(existingCategoryGroup.Name);
            result.Categories.Should().ContainEquivalentOf(existingCategory);
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_DeletesItem()
        {
            //arrange
            var existingItem = new CategoryGroup { Name = "TestCategoryGroup" };
            await _dbContext.CategoryGroups.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var service = new CategoryGroupService(_dbContext, AutoMapperSingleton.Mapper,
                _createCategoryGroupValidator, _updateCategoryGroupValidator);

            //act
            await service.DeleteAsync(existingItem.Id);

            //assert
            var deletedItem = _dbContext.CategoryGroups.FirstOrDefault(x => x.Id == existingItem.Id);
            deletedItem.Should().BeNull();
        }
    }
}
