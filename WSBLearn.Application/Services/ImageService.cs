using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WSBLearn.Application.Dtos;
using WSBLearn.Application.Interfaces;
using WSBLearn.Application.Settings;

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

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            BlobResponseDto response = new();
            BlobContainerClient container = new BlobContainerClient(_storageSettings.ConnectionString, _storageSettings.ContainerName);

            try
            {
                string blobContentType = string.Empty;
                BlobClient client = container.GetBlobClient(blob.FileName);

                if (blob.FileName.EndsWith(".jpg"))
                {
                    blobContentType = "image/jpg";
                }
                else if (blob.FileName.EndsWith(".png"))
                {
                    blobContentType = "image/png";
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
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageSettings.ContainerName}.'");
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

        public async Task<BlobDto?> DownloadAsync(string blobFilename)
        {
            // Get a reference to a container
            BlobContainerClient client = new BlobContainerClient(_storageSettings.ConnectionString, _storageSettings.ContainerName);

            try
            {
                // Get a reference to the blob
                BlobClient file = client.GetBlobClient(blobFilename);

                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    var content = await file.DownloadContentAsync();
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

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

        public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
        {
            BlobContainerClient client = new BlobContainerClient(_storageSettings.ConnectionString, _storageSettings.ContainerName);
            BlobClient file = client.GetBlobClient(blobFilename);

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

        public async Task<List<BlobDto>> ListAsync()
        {
            BlobContainerClient container = new BlobContainerClient(_storageSettings.ConnectionString, _storageSettings.ContainerName);
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
    }


}
