using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Utilities.Extensions;

namespace Utilities.Configurations
{
    public class FireflyConfiguration : IFireflyConfiguration
    {
        private static readonly string KeyVaultTenantIdKey = "";
        private static readonly string KeyVaultNameKey = "";
        private static readonly string KeyVaultUriKey = "";

        private readonly IConfiguration _configuration;
        private readonly TokenCredential _keyVaultCredential;
        private readonly Uri _keyVaultUri;
        private readonly string _keyVaultTenantId;
        private readonly string _KeyVaultName;

        public FireflyConfiguration(IConfiguration configuration) 
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _keyVaultTenantId = _configuration[KeyVaultTenantIdKey] ?? throw new ArgumentNullException(KeyVaultTenantIdKey);
            _KeyVaultName = _configuration[KeyVaultNameKey] ?? throw new ArgumentNullException(KeyVaultNameKey);
            _keyVaultUri =  _configuration[KeyVaultUriKey].TryParseNullOrEmpty(out var s) ? new Uri(s) : throw new ArgumentNullException(KeyVaultUriKey);

            _keyVaultCredential = string.IsNullOrWhiteSpace(_keyVaultTenantId)
                ? new DefaultAzureCredential()
                : new DefaultAzureCredential(new DefaultAzureCredentialOptions { VisualStudioTenantId = _keyVaultTenantId });
        }

        public string? this[string key] 
        { 
            get => _configuration[key]; 
            set => _configuration[key] = value; 
        }

        public string KeyVaultTenantId => _keyVaultTenantId;

        public string KeyVaultName => _KeyVaultName;

        public Uri KeyVaultUri => _keyVaultUri;

        public TokenCredential KeyVaultCredential => _keyVaultCredential;

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _configuration.GetChildren();
        }

        public IEnumerable<IConfigurationSection> GetChildren(string key) 
        { 
            return this.GetChildren().Select(c => c.GetSection(key)); 
        }

        public IConfigurationSection GetConfigurationSection(ConfigurationSectionType section)
        {
            return this.GetSection(section.ToString());
        }

        public IChangeToken GetReloadToken()
        {
            return _configuration.GetReloadToken();
        }

        public IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        public static bool TryParseNullOrEmpty(out string value)
        {
            value = string.Empty;
            return false;
        }
    }
}
