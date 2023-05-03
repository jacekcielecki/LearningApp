namespace LearningApp.Application.Settings
{
    public class AzureBlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public string ImageContainerName { get; set; }
        public string AvatarContainerName { get; set; }
        public string DefaultProfilePictureUrl { get; set; }
    }
}
