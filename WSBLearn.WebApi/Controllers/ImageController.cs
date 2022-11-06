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

        [HttpGet(nameof(Get))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            List<BlobDto>? files = await _imageService.ListAsync();

            return StatusCode(StatusCodes.Status200OK, files);
        }

        [HttpPost(nameof(Upload))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            BlobResponseDto? response = await _imageService.UploadAsync(file);

            if (response.Error == true)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }

            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpGet("{filename}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Download(string filename)
        {
            BlobDto? file = await _imageService.DownloadAsync(filename);

            if (file == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"File {filename} could not be downloaded.");
            }

            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpDelete("filename")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string filename)
        {
            BlobResponseDto response = await _imageService.DeleteAsync(filename);

            if (response.Error == true)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response.Status);
            }

            return StatusCode(StatusCodes.Status200OK, response.Status);
        }

    }
}