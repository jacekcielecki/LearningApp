using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;

namespace WSBLearn.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet("{containerName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync(string containerName = "image")
        {
            List<BlobDto>? files = await _imageService.GetAllAsync(containerName);

            return StatusCode(StatusCodes.Status200OK, files);
        }

        [HttpPost("{containerName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadAsync(IFormFile file, string containerName = "image")
        {
            BlobResponseDto? response = await _imageService.UploadAsync(containerName, file);

            if (response.Error)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);

            return StatusCode(StatusCodes.Status200OK, response);
        }


        [HttpGet("{containerName}/{filename}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByNameAsync(string filename, string containerName = "image")
        {
            BlobDto? file = await _imageService.GetByNameAsync(containerName, filename);

            if (file is null)
                return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");

            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpDelete("{containerName}/{filename}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string filename, string containerName = "image")
        {
            BlobResponseDto response = await _imageService.DeleteAsync(containerName, filename);

            if (response.Error)
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);

            return StatusCode(StatusCodes.Status200OK, response.Status);
        }

    }
}