using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace LearningApp.Application.Tests.Services
{
    public class ImageServiceTests
    {
        private readonly Mock<IBlobStorageService> _blobStorageServiceStub = new Mock<IBlobStorageService>();
        private readonly string _validContainerName = "image";

        [Fact]
        public async Task ListAsync_WithExistingItems_ReturnsAllItems()
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

            var service = new ImageService(_blobStorageServiceStub.Object);

            //act
            var result = await service.ListAsync(_validContainerName);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<BlobDto>>();
            result.Should().BeEquivalentTo(allExistingBlobs);
        }

        [Fact]
        public async Task DownloadAsync_WithValidContainerNameAndFileName_ReturnsItemByName()
        {
            //arrange
            var existingBlob = new BlobDto
            {
                Name = "Test image 1",
                Uri = "https://www.google.com"
            };

            _blobStorageServiceStub
                .Setup(x => x.DownloadAsync(_validContainerName, existingBlob.Name))
                .ReturnsAsync(existingBlob);

            var service = new ImageService(_blobStorageServiceStub.Object);


            //act
            var result = await service.DownloadAsync(_validContainerName, existingBlob.Name);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BlobDto>();
            result.Should().BeEquivalentTo(existingBlob);
        }

        [Fact]
        public async Task UploadAsync_WithContainerNameAndFileToUpload_CreatesItem()
        {
            //arrange
            var blobResponse = new BlobResponseDto
            {
                Error = false,
                Status = "Item Deleted"
            };
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(f => f.FileName).Returns("testFile.jpg");

            _blobStorageServiceStub
                .Setup(x => x.UploadAsync(_validContainerName, It.IsAny<IFormFile>(),
                    It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(blobResponse);

            var service = new ImageService(_blobStorageServiceStub.Object);

            //act
            var result = await service.UploadAsync(_validContainerName, formFileMock.Object);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BlobResponseDto>();
            result.Should().BeEquivalentTo(blobResponse);
        }

        [Fact]
        public async Task DeleteAsync_WithValidContainerNameAndFileName_DeletesItemByName()
        {
            //arrange
            var blobToDelete = new BlobDto
            {
                Name = "Test image 1",
                Uri = "https://www.google.com"
            };
            var blobResponse = new BlobResponseDto
            {
                Error = false,
                Status = "Item Deleted"
            };

            _blobStorageServiceStub
                .Setup(x => x.DeleteAsync(_validContainerName, blobToDelete.Name))
                .ReturnsAsync(blobResponse);

            var service = new ImageService(_blobStorageServiceStub.Object);

            //act
            var result = await service.DeleteAsync(_validContainerName, blobToDelete.Name);

            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType<BlobResponseDto>();
            result.Should().BeEquivalentTo(blobResponse);
        }
    }
}
