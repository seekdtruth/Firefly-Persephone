using System.Configuration;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Environment = Firefly.Core.Configurations.Environment;
using Firefly.Core.Configurations;

namespace IntegrationTests.Helpers
{
    internal class TestFireflyConfiguration : IFireflyConfiguration
    {
        private readonly string keyVaultTenantId;
        private readonly string keyVaultName;
        private readonly Uri keyVaultUri;
        private readonly TokenCredential keyVaultCredential;

        private readonly Environment environment = Environment.Integration;


        public TestFireflyConfiguration()
        {
            keyVaultName = ConfigurationManager.AppSettings[""] ?? throw new ArgumentNullException(nameof(KeyVaultName));
            keyVaultUri = new Uri(ConfigurationManager.AppSettings[""] ?? throw new ArgumentNullException(nameof(KeyVaultUri)));

            keyVaultTenantId = ConfigurationManager.AppSettings[""] ?? string.Empty;
            keyVaultCredential = string.IsNullOrWhiteSpace(keyVaultTenantId)
                ? new DefaultAzureCredential()
                : new DefaultAzureCredential(new DefaultAzureCredentialOptions { VisualStudioTenantId = keyVaultTenantId });
        }

        public string this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string KeyVaultTenantId => keyVaultTenantId;

        public string KeyVaultName => keyVaultName;

        public Uri KeyVaultUri => keyVaultUri;

        public TokenCredential KeyVaultCredential => keyVaultCredential;

        public Environment Environment => environment;

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
