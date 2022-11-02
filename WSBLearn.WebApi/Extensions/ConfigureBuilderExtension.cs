using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace WSBLearn.WebApi.Extensions
{
    public static class ConfigureBuilderExtension
    {
        public static void ConfigureKeyVault(this IConfigurationBuilder config)
        { 
            string? keyVaultEndpoint = Environment.GetEnvironmentVariable("WSB-Learn-KeyVault-Endpoint");
            
            if (keyVaultEndpoint is null)
                throw new InvalidOperationException("WSB-Learn-KeyVault-Endpoint environment variable missing");

            var secretClient = new SecretClient(new(keyVaultEndpoint), new DefaultAzureCredential());
            config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }
    }
}
