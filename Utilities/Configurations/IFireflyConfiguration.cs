using Azure.Core;
using Microsoft.Extensions.Configuration;

namespace Utilities.Configurations
{
    public interface IFireflyConfiguration : IConfiguration
    {
        public IConfigurationSection GetConfigurationSection(ConfigurationSectionType section);

        public string KeyVaultTenantId { get; }

        public string KeyVaultName { get; }

        public Uri KeyVaultUri { get; }

        public TokenCredential KeyVaultCredential { get; }
    }
}