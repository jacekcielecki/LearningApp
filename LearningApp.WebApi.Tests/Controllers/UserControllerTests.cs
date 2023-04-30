using Azure.Core;
using LearningApp.Application.Requests.User;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;

        public UserControllerTests()
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
            //arrange
            var existingItem = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword"
            };
            await _databaseSeeder.Seed(existingItem);

            //act
            var response = await _client.GetAsync("api/User");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingItem_ReturnsItemById()
        {
            //arrange
            var existingRole = new Role { Name = "TestRoleName" };
            var existingUser = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword",
                Role = existingRole,
            };
            await _databaseSeeder.Seed(existingUser);

            //act
            var response = await _client.GetAsync($"api/User/{existingUser.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetSortByExpAsync_WithExistingItems_ReturnsItems()
        {
            //arrange
            var existingItem = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword"
            };
            await _databaseSeeder.Seed(existingItem);

            //act
            var response = await _client.GetAsync("api/User/SortByExp");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateUserAsync_WithValidItemToUpdate_ReturnsStatusOk()
        {
            //arrange
            var itemToUpdate = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword",
            };
            await _databaseSeeder.Seed(itemToUpdate);

            var updatedItem = new UpdateUserRequest
            {
                Username = "UpdatedUserUsername",
                EmailAddress = "TestUser@mail.com",
            };

            //act
            var response = await _client.PatchAsync($"api/User/{itemToUpdate.Id}", updatedItem.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateUserRoleAsync_WithValidUserIdAndRoleId_ReturnsStatusOk()
        {
            //arrange
            var existingUser = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword",
                Role = new Role { Name = "TestUserRoleName" }
            };
            var newRole = new Role { Name = "UpdatedTestUserRole" };

            await _databaseSeeder.Seed(existingUser);
            await _databaseSeeder.Seed(newRole);

            //act
            var response = await _client.PatchAsync($"api/User/updateRole/{existingUser.Id}/{newRole.Id}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateUserPasswordAsync_WithItemToUpdate_ReturnsStatusOk()
        {
            //arrange
            PasswordHasher<User> passwordHasher = new();

            var itemToUpdate = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Role = new Role { Name = "TestUserRoleName" }
            };
            itemToUpdate.Password = passwordHasher.HashPassword(itemToUpdate, "TestUserPassword");

            await _databaseSeeder.Seed(itemToUpdate);

            var updatedItem = new UpdateUserPasswordRequest
            {
                OldPassword = "TestUserPassword",
                NewPassword = "UpdatedTestUserPassword",
                ConfirmNewPassword = "UpdatedTestUserPassword"
            };

            //act
            var response = await _client.PatchAsync($"api/User/updatePassword/{itemToUpdate.Id}",
                updatedItem.ToJsonHttpContent());

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteUserAsync_WithItemToDelete_ReturnsStatusOk()
        {
            //arrange
            var itemToDelete = new User
            {
                Username = "TestUserUsername",
                EmailAddress = "TestUser@mail.com",
                Password = "TestUserPassword"
            };
            await _databaseSeeder.Seed(itemToDelete);

            //act
            var response = await _client.DeleteAsync($"api/User/{itemToDelete.Id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
