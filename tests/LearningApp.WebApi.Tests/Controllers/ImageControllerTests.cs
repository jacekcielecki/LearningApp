using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.WebApi.Tests.Helpers;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;

namespace LearningApp.WebApi.Tests.Controllers
{
    public class ImageControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly Mock<IBlobStorageService> _blobStorageServiceStub = new Mock<IBlobStorageService>();
        private readonly string _validContainerName = "image";

        public ImageControllerTests(WebApplicationFactory<Program> factory)
        {
           factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddSingleton<IBlobStorageService>(_blobStorageServiceStub.Object);
                });
            });

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ListAsync_WithValidContainerName_ReturnsStatusOk()
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

            _blobStorageServiceStub
                .Setup(x => x.ListAsync(It.IsAny<string>()))
                .ReturnsAsync(allExistingBlobs);

            //act
            var response = await _client.GetAsync($"api/Image/{_validContainerName}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.DeserializeHttpContent<List<BlobDto>>().Should().BeOfType<List<BlobDto>>();
        }

        [Fact]
        public async Task DownloadAsync_WithValidContainerNameAndFileName_ReturnsStatusOk()
        {
            //arrange
            var existingBlob = new BlobDto()
            {
                Name = "testFileName.jpg",
                Content = new MemoryStream(Encoding.UTF8.GetBytes("test file content")),
                ContentType = "image/jpeg"
            };

            _blobStorageServiceStub
                .Setup(x => x.DownloadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(existingBlob);

            // Act
            var response = await _client.GetAsync($"api/Image/{_validContainerName}/{existingBlob.Name}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task UploadAsync_WithValidContainerNameAndImageFile_ReturnsStatusOk()
        {
            // Arrange
            var filename = "testFile.jpg";

            _blobStorageServiceStub.Setup(x => x.UploadAsync(
                    It.IsAny<string>(), It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new BlobResponseDto
                {
                    Error = false,
                    Status = "File successfully uploaded",
                    Blob = new BlobDto
                    {
                        Uri = "https://www.google.pl",
                        Name = filename,
                    }
                });

            await using var testFile = File.OpenRead(@"TestFiles\test-image.png");
            using var content = new StreamContent(testFile);
            using var formData = new MultipartFormDataContent();
            formData.Add(content, "file", filename);

            // Act
            var response = await _client.PostAsync($"api/Image/{_validContainerName}", formData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_WithValidFileAndContainerName_ReturnsOk()
        {
            //arrange
            var filename = "TestFileName3";
            _blobStorageServiceStub.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
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
