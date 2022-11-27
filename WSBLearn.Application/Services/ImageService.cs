using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Settings;
using WSBLearn.Application.Exceptions;

namespace WSBLearn.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _logger;
        private readonly AzureBlobStorageSettings _storageSettings;

        public ImageService(ILogger<ImageService> logger, AzureBlobStorageSettings storageSettings)
        {
            _logger = logger;
            _storageSettings = storageSettings;
        }

        public async Task<BlobResponseDto> UploadAsync(string containerName, IFormFile blob)
        {
            BlobResponseDto response = new();
            var container = GetBlobContainerClient(containerName);

            try
            {
                var blobContentType = string.Empty;
                var fileName = Path.GetFileNameWithoutExtension(blob.FileName).ToString();
                var extension = Path.GetExtension(blob.FileName);
                var randomFileName = fileName + "-" + Guid.NewGuid().ToString() + extension;
                BlobClient client = container.GetBlobClient(randomFileName);

                if (blob.FileName.EndsWith(".jpg"))
                {
                    blobContentType = "image/jpg";
                }
                else if (blob.FileName.EndsWith(".png"))
                {
                    blobContentType = "image/png";
                }
                else if (blob.FileName.EndsWith(".svg"))
                {
                    blobContentType = "image/svg+xml";
                }
                else
                {
                    throw new InvalidFileTypeException("File extension must be .jpg, .svg or .png");
                }

                var options = new BlobUploadOptions() { HttpHeaders = new BlobHttpHeaders() {ContentType = blobContentType } };

                await using (Stream? data = blob.OpenReadStream())
                {
                    await client.UploadAsync(data, options);
                }

                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;
            }

            // If the file already exists, catch the exception and do not upload it
            catch (RequestFailedException ex)
               when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageSettings.ImageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            return response;
        }

        public async Task<BlobDto?> GetByNameAsync(string containerName, string blobFilename)
        {
            var container = GetBlobContainerClient(containerName);

            try
            {
                BlobClient file = container.GetBlobClient(blobFilename);

                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    var content = await file.DownloadContentAsync();
                    var name = blobFilename;
                    var contentType = content.Value.Details.ContentType;

                    return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError($"File {blobFilename} was not found.");
            }

            return null;
        }

        public async Task<BlobResponseDto> DeleteAsync(string containerName, string blobFilename)
        {
            var container = GetBlobContainerClient(containerName);
            BlobClient file = container.GetBlobClient(blobFilename);

            try
            {
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError($"File {blobFilename} was not found.");
                return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };
        }

        public async Task<List<BlobDto>> GetAllAsync(string containerName)
        {
            var container = GetBlobContainerClient(containerName);
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            return files;
        }

        private BlobContainerClient GetBlobContainerClient(string containerName)
        {
            switch (containerName)
            {
                case "image":
                    return new BlobContainerClient(_storageSettings.ConnectionString, _storageSettings.ImageContainerName);
                case "avatar":
                    return new BlobContainerClient(_storageSettings.ConnectionString, _storageSettings.AvatarContainerName);
                default:
                    throw new NotSupportedException("Container with given name does not exist");
            }
        }
    }


}
