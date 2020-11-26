using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Website
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureKeyVault(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var vaultName = configBuilder.Build()["KeyVaultName"];

                if (!string.IsNullOrEmpty(vaultName))
                {
                    var authCallback = new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback);
                    var keyVaultClient = new KeyVaultClient(authCallback);

                    configBuilder.AddAzureKeyVault(
                        $"https://{vaultName}.vault.azure.net", keyVaultClient, new DefaultKeyVaultSecretManager());
                }
            });
        }
    }
}
