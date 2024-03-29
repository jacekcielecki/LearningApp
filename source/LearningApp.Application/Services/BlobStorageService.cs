﻿using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LearningApp.Application.Dtos;
using LearningApp.Application.Interfaces;
using LearningApp.Application.Settings;
using LearningApp.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LearningApp.Application.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly ILogger<BlobStorageService> _logger;
        private readonly AzureBlobStorageSettings _storageSettings;

        public BlobStorageService(ILogger<BlobStorageService> logger, AzureBlobStorageSettings storageSettings)
        {
            _logger = logger;
            _storageSettings = storageSettings;
        }

        public async Task<BlobResponseDto> UploadAsync(string containerName, IFormFile file, string trustedFileName, string contentType)
        {
            BlobResponseDto response = new();
            var container = GetBlobContainerClient(containerName);

            try
            {
                BlobClient client = container.GetBlobClient(trustedFileName);
                var options = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } };

                await using (Stream data = file.OpenReadStream())
                {
                    await client.UploadAsync(data, options);
                }

                response.Status = BlobStorageMessages.FileUploadedSuccessfully(file.FileName);
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;
            }

            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError(BlobStorageMessages.FileNameTaken(file.FileName, _storageSettings.ImageContainerName));
                response.Status = BlobStorageMessages.FileNameTaken(file.FileName, _storageSettings.ImageContainerName);
                response.Error = true;
                return response;
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(BlobStorageMessages.UnhandledException(ex.StackTrace, ex.Message));
                response.Status = BlobStorageMessages.UnhandledException(ex.StackTrace, ex.Message);
                response.Error = true;
                return response;
            }

            return response;
        }

        public async Task<BlobDto> DownloadAsync(string containerName, string blobFilename)
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
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError(BlobStorageMessages.FileNotFound(blobFilename));
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
            catch (RequestFailedException ex) when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                _logger.LogError(BlobStorageMessages.FileNotFound(blobFilename));
                return new BlobResponseDto { Error = true, Status = BlobStorageMessages.FileNotFound(blobFilename) };
            }

            return new BlobResponseDto { Error = false, Status = BlobStorageMessages.FileDeletedSuccessfully(blobFilename) };
        }

        public async Task<List<BlobDto>> ListAsync(string containerName)
        {
            var container = GetBlobContainerClient(containerName);
            var files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                var uri = container.Uri.ToString();
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
            return containerName switch
            {
                "image" => new BlobContainerClient(_storageSettings.ConnectionString,
                    _storageSettings.ImageContainerName),
                "avatar" => new BlobContainerClient(_storageSettings.ConnectionString,
                    _storageSettings.AvatarContainerName),
                _ => throw new NotSupportedException(BlobStorageMessages.InvalidContainer)
            };
        }
    }


}
