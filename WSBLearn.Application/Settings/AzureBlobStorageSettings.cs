namespace WSBLearn.Application.Settings
{
    public class AzureBlobStorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ImageContainerName { get; set; } = string.Empty;
        public string AvatarContainerName { get; set; } = string.Empty;
    }
}
