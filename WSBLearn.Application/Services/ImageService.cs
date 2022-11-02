using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using WSBLearn.Application.Interfaces;

namespace WSBLearn.Application.Services
{
    public class ImageService : IImageService
    {
        private readonly ILogger<ImageService> _logger;
        private readonly AzureBlobStorageSettings _azureBlobStorageSettings;
       // private readonly CloudStorageAccount _cloudStorageAccount;

        public ImageService(ILogger<ImageService> logger, AzureBlobStorageSettings azureBlobStorageSettings)
        {
            _logger = logger;
            _azureBlobStorageSettings = azureBlobStorageSettings;
            //_cloudStorageAccount = CloudStorageAccount.Parse(_azureBlobStorageSettings.ConnectionString);
        }

        public async Task UploadToAzureAsync(IFormFile file)
        {
            //var cloudBlobClient = _cloudStorageAccount.CreateCloudBlobClient();
            //var cloudBlobContainer = cloudBlobClient.GetContainerReference(_azureBlobStorageSettings.ContainerName);

            //if (await cloudBlobContainer.CreateIfNotExistsAsync())
            //{
            //    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions{ PublicAccess = BlobContainerPublicAccessType.Off });
            //}

            //var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(file.FileName);
            //cloudBlockBlob.Properties.ContentType = file.ContentType;

            //await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());
        }
    }
}
