using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.User;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IUserService> _userServiceStub = new Mock<IUserService>();
        private readonly DatabaseSeeder _databaseSeeder;

        public AccountControllerTests(WebApplicationFactory<Program> factory)
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
                    services.AddSingleton<IUserService>(_userServiceStub.Object);
                });
            });

            _client = factory.CreateClient();
            _databaseSeeder = new DatabaseSeeder(factory);
        }

        [Fact]
        public async Task RegisterUser_WithValidItemToCreate_ReturnsStatusOk()
        {
            //arrange
            var newUser = new CreateUserRequest()
            {
                EmailAddress = "newuser@mail.com",
                Username = "newUser",
                Password = "newUserPassword",
                ConfirmPassword = "newUserPassword"
            };

            //act
            var response = await _client.PostAsync("/api/Account/register", newUser.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsJwtToken()
        {
            //arrange
            _userServiceStub
                .Setup(x => x.LoginAsync(It.IsAny<LoginDto>()))
                .ReturnsAsync("testJwtToken");

            var existingUser = new User
            {
                EmailAddress = "existingUser@mail.com",
                Username = "existingUser",
                Password = "existingUserPassword",
            };
            await _databaseSeeder.Seed(existingUser);

            var loginCredentials = new LoginDto
            {
                Login = existingUser.EmailAddress,
                Password = existingUser.Password
            };

            //act
            var response = await _client.PostAsync("api/Account/login", loginCredentials.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
