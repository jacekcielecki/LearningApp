using Microsoft.AspNetCore.Http;

namespace WSBLearn.Application.Interfaces
{
    public interface IImageService
    { 
        Task UploadToAzureAsync(IFormFile file);
    }
}
