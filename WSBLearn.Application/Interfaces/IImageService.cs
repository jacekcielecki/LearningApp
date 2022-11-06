using Microsoft.AspNetCore.Http;
using WSBLearn.Application.Dtos;


namespace WSBLearn.Application.Interfaces
{
    public interface IImageService
    {
        Task<BlobResponseDto> UploadAsync(IFormFile file);
        Task<BlobDto?> DownloadAsync(string blobFilename);
        Task<BlobResponseDto> DeleteAsync(string blobFilename);
        Task<List<BlobDto>> ListAsync();
    }
}
