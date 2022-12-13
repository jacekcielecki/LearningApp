using Microsoft.AspNetCore.Http;
using WSBLearn.Application.Dtos;


namespace WSBLearn.Application.Interfaces
{
    public interface IImageService
    {
        Task<List<BlobDto>> GetAllAsync(string containerName);
        Task<BlobDto?> GetByNameAsync(string containerName, string blobFilename);
        Task<BlobResponseDto> UploadAsync(string containerName, IFormFile file);
        Task<BlobResponseDto> DeleteAsync(string containerName, string blobFilename);
    }
}
