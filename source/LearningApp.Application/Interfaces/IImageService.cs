using LearningApp.Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace LearningApp.Application.Interfaces
{
    public interface IImageService
    {
        Task<List<BlobDto>> ListAsync(string containerName);
        Task<BlobDto> DownloadAsync(string containerName, string blobFilename);
        Task<BlobResponseDto> UploadAsync(string containerName, IFormFile file);
        Task<BlobResponseDto> DeleteAsync(string containerName, string blobFilename);
    }
}
