using Microsoft.AspNetCore.Http;
using WSBLearn.Application.Dtos;


namespace WSBLearn.Application.Interfaces
{
    public interface IImageService
    {
        Task<BlobResponseDto> UploadAsync(string containerName, IFormFile file);
        Task<BlobDto?> GetByNameAsync(string containerName, string blobFilename);
        Task<BlobResponseDto> DeleteAsync(string containerName, string blobFilename);
        Task<List<BlobDto>> GetAllAsync(string containerName);
    }
}
