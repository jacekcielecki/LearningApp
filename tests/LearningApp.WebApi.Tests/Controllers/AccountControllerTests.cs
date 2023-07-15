using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Requests.User;
using LearningApp.Domain.Entities;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IPasswordHasher<User>> _passwordHasherMock = new Mock<IPasswordHasher<User>>();
        private readonly Mock<IEmailService> _emailServiceStub = new Mock<IEmailService>();
        private readonly DatabaseSeeder _databaseSeeder;

        public AccountControllerTests(WebApplicationFactory<Program> factory)
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
                    services.AddDbContext<LearningAppDbContext>(options => options.UseInMemoryDatabase("AccountControllerTests"));
                    services.AddSingleton<IEmailService>(_emailServiceStub.Object);
                    services.AddSingleton<IPasswordHasher<User>>(_passwordHasherMock.Object);
                });
            });

            _client = factory.CreateClient();
            _databaseSeeder = new DatabaseSeeder(factory);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsStatusOk()
        {
            //arrange
            var existingUser = new User
            {
                EmailAddress = "existingUser@mail.com",
                Username = "existingUser",
                Password = "existingUserPassword",
                Role = new Role { Name = "User" }
            };
            await _databaseSeeder.Seed(existingUser);
            var loginCredentials = new LoginDto
            {
                Email = existingUser.EmailAddress,
                Password = existingUser.Password
            };

            _passwordHasherMock
                .Setup(x => x.VerifyHashedPassword(
                    It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            //act
            var response = await _client.PostAsync("api/Account/login", loginCredentials.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
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

            var hashedPasswordMock = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)); ;
            _passwordHasherMock.Setup(ph => ph.HashPassword(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(hashedPasswordMock);

            //act
            var response = await _client.PostAsync("/api/Account/register", newUser.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task VerifyAccount_WithValidVerificationToken_ReturnsStatusOk()
        {
            //arrange
            var existingUser = new User
            {
                EmailAddress = "existingUser@mail.com",
                Username = "existingUser",
                Password = "existingUserPassword",
                VerificationToken = "testVerificationToken",
                VerificationTokenExpireDate = DateTime.Now.AddDays(1),
                Role = new Role { Name = "User" }
            };
            await _databaseSeeder.Seed(existingUser);

            //act
            var response = await _client.GetAsync($"api/Account/verify?verificationToken={existingUser.VerificationToken}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task SendAccountVerificationEmail_WithValidUserEmail_ReturnsStatusOk()
        {
            //arrange
            var userEmail = "testuser@mail.com";

            //act
            var response = await _client.PatchAsync($"api/Account/sendVerificationEmail?userEmail={userEmail}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPasswordResetToken_WithExistingUserEmail_ReturnsStatusOk()
        {
            //arrange
            var existingUser = new User
            {
                EmailAddress = "existingUser@mail.com",
                Username = "existingUser",
                Password = "existingUserPassword"
            };
            await _databaseSeeder.Seed(existingUser);

            //act
            var response = await _client.PatchAsync($"api/Account/forget-password?userEmail={existingUser.EmailAddress}", null);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task ResetPassword_WithValidResetPasswordRequest_ReturnsStatusOk()
        {
            //arrange
            var existingUser = new User
            {
                EmailAddress = "existingUser@mail.com",
                Username = "existingUser",
                Password = "existingUserPassword",
                ResetPasswordToken = "testResetPasswordToken",
                ResetPasswordTokenExpireDate = DateTime.Now.AddDays(1),
                Role = new Role { Name = "User" }
            };
            await _databaseSeeder.Seed(existingUser);
            var request = new ResetPasswordRequest
            {
                Password = "newPassword",
                ConfirmPassword = "newPassword",
                Token = existingUser.ResetPasswordToken
            };

            //act
            var response = await _client.PatchAsync("api/Account/reset-password", request.ToJsonHttpContent());

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
