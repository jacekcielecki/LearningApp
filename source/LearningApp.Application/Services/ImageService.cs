using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace LearningApp.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IBlobStorageService _blobStorageService;

        public ImageService(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public Task<List<BlobDto>> GetAllAsync(string containerName)
        {
            return _blobStorageService.GetAllAsync(containerName);
        }

        public Task<BlobDto> GetByNameAsync(string containerName, string blobFilename)
        {
            return _blobStorageService.GetByNameAsync(containerName, blobFilename);
        }

        public Task<BlobResponseDto> UploadAsync(string containerName, IFormFile file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);
            var trustedFileName = fileName + "-" + Guid.NewGuid() + fileExtension;
            var fileContentType = GetFileContentType(fileExtension);

            return _blobStorageService.UploadAsync(containerName, file, trustedFileName, fileContentType);
        }

        public Task<BlobResponseDto> DeleteAsync(string containerName, string blobFilename)
        {
            return _blobStorageService.DeleteAsync(containerName, blobFilename);
        }

        private string GetFileContentType(string fileName) =>
            fileName switch
            {
                ".jpg" => "image/jpg",
                ".png" => "image/png",
                ".svg" => "image/svg+xml",
                _ => throw new InvalidFileTypeException()
            };
    }
}
