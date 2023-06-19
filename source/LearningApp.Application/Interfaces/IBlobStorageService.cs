using LearningApp.Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace LearningApp.Application.Interfaces
{
    public interface IBlobStorageService
    {
        Task<List<BlobDto>> GetAllAsync(string containerName);
        Task<BlobDto> GetByNameAsync(string containerName, string blobFilename);
        Task<BlobResponseDto> UploadAsync(string containerName, IFormFile file, string trustedFileName, string contentType);
        Task<BlobResponseDto> DeleteAsync(string containerName, string blobFilename);
    }
}
