using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.Achievement;
using LearningApp.Application.Services;
using LearningApp.Application.Tests.Helpers;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;

namespace LearningApp.Application.Tests.Services
{
    public class AchievementServiceTests
    {
        private readonly WsbLearnDbContext _dbContext;
        private readonly IValidator<CreateAchievementRequest> _createAchievementRequestValidator = new CreateAchievementRequestValidator();
        private readonly IValidator<UpdateAchievementRequest> _updateAchievementRequestValidator = new UpdateAchievementRequestValidator();

        public AchievementServiceTests()
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
            var existingItems = new List<Achievement>
            {
                new() { Name = "TestAchievementName" },
                new() { Name = "TestAchievementName" },
                new() { Name = "TestAchievementName" }
            };
            await _dbContext.Achievements.AddRangeAsync(existingItems);
            await _dbContext.SaveChangesAsync();

            var service = new AchievementService(_dbContext, AutoMapperSingleton.Mapper, _createAchievementRequestValidator, _updateAchievementRequestValidator);

            //act
            var result = await service.GetAllAsync();

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<AchievementDto>>();
            result.Should().ContainEquivalentOf(existingItems.FirstOrDefault(), options => options.ComparingByMembers<AchievementDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task CreateAsync_WithValidItemToCreate_ReturnsCreatedItem()
        {
            //arrange
            var itemToCreate = new CreateAchievementRequest { Name = "TestAchievementName", Description = "TestAchievementDescription"};
            var service = new AchievementService(_dbContext, AutoMapperSingleton.Mapper, _createAchievementRequestValidator, _updateAchievementRequestValidator);

            //act
            var result = await service.CreateAsync(itemToCreate);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AchievementDto>();
            result.Should().BeEquivalentTo(itemToCreate,
                options => options.ComparingByMembers<AchievementDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdate_ReturnsUpdatedItem()
        {
            //arrange
            var existingItem = new Achievement { Name = "TestAchievementName"};
            await _dbContext.Achievements.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var updatedItem = new UpdateAchievementRequest { Name = "UpdatedTestAchievementName", Description = "UpdatedTestAchievementDescription" };
            var service = new AchievementService(_dbContext, AutoMapperSingleton.Mapper, _createAchievementRequestValidator, _updateAchievementRequestValidator);

            //act
            var result = await service.UpdateAsync(existingItem.Id, updatedItem);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AchievementDto>();
            result.Should().BeEquivalentTo(updatedItem,
                options => options.ComparingByMembers<AchievementDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_DeletesItem()
        {
            //arrange
            var existingItem = new Achievement { Name = "TestAchievementName" };
            await _dbContext.Achievements.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var service = new AchievementService(_dbContext, AutoMapperSingleton.Mapper, _createAchievementRequestValidator, _updateAchievementRequestValidator);

            //act
            await service.DeleteAsync(existingItem.Id);

            //assert
            var deletedItem = _dbContext.Achievements.FirstOrDefault(x => x.Id == existingItem.Id);
            deletedItem.Should().BeNull();
        }
    }
}
