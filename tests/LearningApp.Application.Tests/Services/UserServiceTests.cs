using LearningApp.Application.Dtos;
using LearningApp.Application.Requests.User;
using LearningApp.Application.Services;
using LearningApp.Application.Settings;
using LearningApp.Application.Tests.Helpers;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace LearningApp.Application.Tests.Services
{
    public class UserServiceTests
    {
        private readonly LearningAppDbContext _dbContext;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock;
        private readonly IValidator<CreateUserRequest> _createUserValidator;
        private readonly IValidator<UpdateUserRequest> _updateUserValidator;
        private readonly IValidator<UpdateUserPasswordRequest> _updateUserPasswordValidator;
        private readonly JwtAuthenticationSettings _jwtAuthenticationSettings;
        private readonly AzureBlobStorageSettings _azureBlobStorageSettings;

        public UserServiceTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<LearningAppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new LearningAppDbContext(dbContextOptions);
            _passwordHasherMock = new Mock<IPasswordHasher<User>>();
            _createUserValidator = new CreateUserRequestValidator(_dbContext);
            _updateUserValidator = new UpdateUserRequestValidator(_dbContext);
            _updateUserPasswordValidator = new UpdateUserPasswordRequestValidator();
            _jwtAuthenticationSettings = new JwtAuthenticationSettings
            {
                ExpireDays = 2,
                Issuer = "TestIssuer",
                Key = "$@4bg24Tuv19F3!I&8n"
            };
            _azureBlobStorageSettings = new AzureBlobStorageSettings
            {
                AvatarContainerName = "AvatarTest",
                ConnectionString = "TestConnectionString",
                DefaultProfilePictureUrl = "https://www.testurl.pl",
                ImageContainerName = "ImageTest"
            };

        }

        [Fact]
        public async Task RegisterAsync_WithValidItemToCreate_CreatesNewItem()
        {
            //arrange
            var testPassword = "TestUserPassword";
            var itemToCreate = new CreateUserRequest
            {
                Username = "NewTestUserUsername",
                EmailAddress = "NewTestUserEmail@mail.com",
                Password = testPassword,
                ConfirmPassword = testPassword,
                ProfilePictureUrl = "https://www.testurl.pl"
            };

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("HashedPasswordMock");

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            //act
            await service.RegisterAsync(itemToCreate);

            //assert
            var check = _dbContext.Users.FirstOrDefault(x => x.EmailAddress == itemToCreate.EmailAddress);
            check.Should().NotBeNull();
            check?.Password.Should().NotBe(testPassword);
        }

        [Fact]
        public async Task LoginAsync_WithValidItem_ReturnsJwtToken()
        {
            //arrange
            var testPassword = "TestUserPassword";
            var existingUser = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUserEmail@mail.com",
                Password = testPassword,
                Role = new Role { Name = "TestUserRole" }
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var loginDto = new LoginDto { Email = existingUser.EmailAddress, Password = testPassword };

            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            //act
            var result = await service.LoginAsync(loginDto);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<string>();
        }

        [Fact]
        public async Task GetAllAsync_WithExistingItems_ReturnsItems()
        {
            //arrange
            var existingItems = new List<User>
            {
                new() {
                    Username = "TestUserUsername1",
                    EmailAddress = "TestUser1@mail.com",
                    Password = "TestUserPassword1",
                    Role = new Role { Name = "TestRole" }
                },
                new() {
                    Username = "TestUserUsername2",
                    EmailAddress = "TestUser2@mail.com",
                    Password = "TestUserPassword2",
                    Role = new Role { Name = "TestRole" }
                },

            };
            await _dbContext.Users.AddRangeAsync(existingItems);
            await _dbContext.SaveChangesAsync();

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            //act
            var result = await service.GetAllAsync();

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<UserDto>>();
            result.Should().ContainEquivalentOf(existingItems.FirstOrDefault(), 
                options => options.ComparingByMembers<UserDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingItem_ReturnsItemById()
        {
            //arrange
            var existingItem = new User
            {
                Username = "TestUserUsername1",
                EmailAddress = "TestUser1@mail.com",
                Password = "TestUserPassword1",
                Role = new Role { Name = "TestRole" }
            };
            await _dbContext.Users.AddAsync(existingItem);
            await _dbContext.SaveChangesAsync();

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            //act
            var result = await service.GetByIdAsync(existingItem.Id);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<UserDto>();
            result.Should().BeEquivalentTo(existingItem,
                options => options.ComparingByMembers<UserDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetSortByExpAsync_WithExistingItems_ReturnsItemsOrderedByTotalExp()
        {
            //arrange
            var existingItems = new List<User>
            {
                new() {
                    Username = "TestUserUsername1",
                    EmailAddress = "TestUser1@mail.com",
                    Password = "TestUserPassword1",
                    Role = new Role { Name = "TestRole" },
                },
                new() {
                    Username = "TestUserUsername2",
                    EmailAddress = "TestUser2@mail.com",
                    Password = "TestUserPassword2",
                    Role = new Role { Name = "TestRole" }
                }
            };
            await _dbContext.Users.AddRangeAsync(existingItems);
            await _dbContext.SaveChangesAsync();

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            //act
            var result = await service.GetAllAsync();

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<UserDto>>();
            result.Should().ContainEquivalentOf(existingItems.FirstOrDefault(), 
                options => options.ComparingByMembers<UserDto>().ExcludingMissingMembers());
        }

        [Fact]
        public async Task UpdateAsync_WithValidItemToUpdate_ReturnsUpdatedItem()
        {
            //arrange
            var itemToUpdate = new User()
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword",
                ProfilePictureUrl = "https://www.testurl.pl",
                Role = new Role { Name = "TestRole" },
            };
            await _dbContext.Users.AddAsync(itemToUpdate);
            await _dbContext.SaveChangesAsync();

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            var updatedItem = new UpdateUserRequest
            {
                Username = "UpdatedTestUserUsername",
                EmailAddress = "UpdatedTestUser@mail.com"
            };

            //act
            var response = await service.UpdateAsync(itemToUpdate.Id, updatedItem);

            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<UserDto>();
            response.Id.Should().Be(itemToUpdate.Id);
            response.EmailAddress.Should().Be(updatedItem.EmailAddress);
        }

        [Fact]
        public async Task UpdateUserRoleAsync_WithValidRoleId_ReturnsUpdatedItem()
        {
            //arrange
            var existingUserRole = new Role { Name = "TestRole" };
            var newUserRole = new Role { Name = "UpdatedTestRole" };
            await _dbContext.Roles.AddAsync(existingUserRole);
            await _dbContext.Roles.AddAsync(newUserRole);
            await _dbContext.SaveChangesAsync();

            var existingUser = new User()
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword",
                ProfilePictureUrl = "https://www.testurl.pl",
                RoleId = existingUserRole.Id,
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            //act
            var response = await service.UpdateUserRoleAsync(existingUser.Id, newUserRole.Id);

            //assert
            response.Should().NotBeNull();
            response.Should().BeOfType<UserDto>();
            response.Id.Should().Be(existingUser.Id);
            response.Role.Id.Should().Be(newUserRole.Id);
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_WithValidNewPassword_UpdatesExistingUserPassword()
        {
            //arrange
            var oldUserPassword = "UserPassword";
            var existingUser = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = oldUserPassword,
                ProfilePictureUrl = "https://www.testurl.pl",
                Role = new Role { Name = "TestRole" },
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            _passwordHasherMock
                .Setup(x => x.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns("HashedPasswordMock");
            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            var updateUserPasswordRequest = new UpdateUserPasswordRequest
            {
                OldPassword = oldUserPassword,
                NewPassword = "UpdatedUserPassword",
                ConfirmNewPassword = "UpdatedUserPassword"
            };

            //act
            await service.UpdateUserPasswordAsync(existingUser.Id, updateUserPasswordRequest);

            //assert
            var check = _dbContext.Users.FirstOrDefault(x => x.Id == existingUser.Id);
            check.Should().NotBeNull();
            check?.EmailAddress.Should().Be(existingUser.EmailAddress);
            check?.Password.Should().NotBe(oldUserPassword);
        }

        [Fact]
        public async Task DeleteAsync_WithItemToDelete_DeletesItem()
        {
            //arrange
            var existingUser = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword",
                Role = new Role { Name = "TestRole" },
            };
            await _dbContext.Users.AddAsync(existingUser);
            await _dbContext.SaveChangesAsync();

            var service = new UserService(_dbContext, _passwordHasherMock.Object, _createUserValidator,
                _updateUserValidator, _updateUserPasswordValidator, _jwtAuthenticationSettings,
                _azureBlobStorageSettings, AutoMapperSingleton.Mapper);

            //act
            await service.DeleteAsync(existingUser.Id);

            //assert
            var check = _dbContext.Users.FirstOrDefault(x => x.Id == existingUser.Id);
            check.Should().BeNull();
        }
    }
}
