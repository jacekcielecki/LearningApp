﻿using LearningApp.Application.Interfaces;
using LearningApp.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.WebApi.Controllers
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
        public async Task<IActionResult> ListAsync(string containerName = "image")
        {
            var files = await _imageService.ListAsync(containerName);
            return Ok(files);
        }

        [HttpGet("{containerName}/{filename}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DownloadAsync(string filename, string containerName = "image")
        {
            var file = await _imageService.DownloadAsync(containerName, filename);
            if (file is null) return StatusCode(StatusCodes.Status500InternalServerError, BlobStorageMessages.FileNotFound(filename));

            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpPost("{containerName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadAsync(IFormFile file, string containerName = "image")
        {
            var response = await _imageService.UploadAsync(containerName, file);
            return response.Error ? StatusCode(StatusCodes.Status500InternalServerError, response.Status) : Ok(response);
        }

        [HttpDelete("{containerName}/{filename}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(string filename, string containerName = "image")
        {
            var response = await _imageService.DeleteAsync(containerName, filename);
            return response.Error ? StatusCode(StatusCodes.Status500InternalServerError, response.Status) : Ok(response.Status);
        }
    }
}