using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Infrastructure.Persistence;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class ImageControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly DatabaseSeeder _databaseSeeder;
        private readonly Mock<IImageService> _imageServiceStub = new Mock<IImageService>();
        private readonly string _validContainerName = "image";

        public ImageControllerTests(WebApplicationFactory<Program> factory)
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
                    services.AddSingleton<IImageService>(_imageServiceStub.Object);
                });
            });

            _client = factory.CreateClient();
            _databaseSeeder = new DatabaseSeeder(factory);
        }

        [Fact]
        public async Task GetAllAsync_WithValidContainerName_ReturnsStatusOk()
        {
            //arrange
            var allExistingBlobs = new List<BlobDto>
            {
                new()
                {
                    Name = "Test image 1",
                    Uri = "https://www.google.com"
                }
            };

            _imageServiceStub
                .Setup(x => x.GetAllAsync(It.IsAny<string>()))
                .ReturnsAsync(allExistingBlobs);

            //act
            var response = await _client.GetAsync($"api/Image/{_validContainerName}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<List<BlobDto>>().Should().BeOfType<List<BlobDto>>();
        }

        [Fact]
        public async Task GetByNameAsync_WithValidContainerNameAndFileName_ReturnsStatusOk()
        {
            var existingBlob = new BlobDto()
            {
                Name = "testFileName.jpg",
                Content = new MemoryStream(Encoding.UTF8.GetBytes("test file content")),
                ContentType = "image/jpeg"
            };

            _imageServiceStub
                .Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(existingBlob);

            // Act
            var response = await _client.GetAsync($"api/Image/{_validContainerName}/{existingBlob.Name}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_WithValidFileAndContainerName_ReturnsOk()
        {
            //arrange
            var filename = "TestFileName3";
            _imageServiceStub.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new BlobResponseDto
                {
                    Error = false,
                    Status = "File successfully deleted",
                    Blob = new BlobDto
                    {
                        Name = filename,
                        Content = new MemoryStream(Encoding.UTF8.GetBytes("test file content")),
                        ContentType = "text/plain"
                    }
                });

            //act
            var response = await _client.DeleteAsync($"api/Image/{_validContainerName}/{filename}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
