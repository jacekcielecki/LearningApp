using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Interfaces;

namespace WSBLearn.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{containerName}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetAllAsync(string containerName = "image")
        {
            var files = await _imageService.GetAllAsync(containerName);
            return Ok(files);
        }

        [HttpGet("{containerName}/{filename}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetByNameAsync(string filename, string containerName = "image")
        {
            var file = await _imageService.GetByNameAsync(containerName, filename);
            if (file is null)
                return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");

            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpPost("{containerName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadAsync(IFormFile file, string containerName = "image")
        {
            var response = await _imageService.UploadAsync(containerName, file);
            if (response.Error)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);

            return Ok(response);
        }

        [HttpDelete("{containerName}/{filename}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string filename, string containerName = "image")
        {
            var response = await _imageService.DeleteAsync(containerName, filename);
            if (response.Error)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);

            return Ok(response.Status);
        }
    }
}