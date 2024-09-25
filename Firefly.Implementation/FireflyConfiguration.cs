using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Core;
using Azure.Identity;
using Firefly.Core;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Utilities.Extensions;
using Environment = Firefly.Core.Environment;

namespace Firefly.Implementation
{
    /// <summary>
    /// Represents an implementation of an <see cref="IFireflyConfiguration"/>
    /// </summary>
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

        private readonly Environment _environment;

        private readonly ILogger<FireflyConfiguration> _logger;

        /// <summary>
        /// Creates an instance of a <see cref="FireflyConfiguration"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null</exception>
        public FireflyConfiguration(IConfiguration configuration, ILoggerFactory? loggerFactory) 
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = EnvironmentExtensions.GetEnvironment();
            loggerFactory = loggerFactory ?? new LoggerFactory();
            _logger = loggerFactory.CreateLogger<FireflyConfiguration>();

            _keyVaultTenantId = TryParseConfigurationValue(KeyVaultTenantIdKey, out var tenant) ? tenant : throw new ArgumentNullException(KeyVaultTenantIdKey);
            _KeyVaultName = TryParseConfigurationValue(KeyVaultNameKey, out var name) ? name : throw new ArgumentNullException(KeyVaultNameKey);
            _keyVaultUri =  TryParseConfigurationValue(KeyVaultUriKey, out var uri) ? new Uri(uri) : throw new ArgumentNullException(KeyVaultUriKey);

            _keyVaultCredential = string.IsNullOrWhiteSpace(_keyVaultTenantId)
                ? new DefaultAzureCredential()
                : new DefaultAzureCredential(new DefaultAzureCredentialOptions { VisualStudioTenantId = _keyVaultTenantId });
        }

        /// <inheritdoc/>
        public string? this[string key] 
        { 
            get => _configuration[key]; 
            set => _configuration[key] = value; 
        }

        /// <inheritdoc/>
        public string KeyVaultTenantId => _keyVaultTenantId;

        /// <inheritdoc/>
        public string KeyVaultName => _KeyVaultName;

        /// <inheritdoc/>
        public Uri KeyVaultUri => _keyVaultUri;

        /// <inheritdoc/>
        public TokenCredential KeyVaultCredential => _keyVaultCredential;

        /// <inheritdoc/>
        public Environment Environment => _environment;

        /// <inheritdoc/>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _configuration.GetChildren();
        }

        /// <inheritdoc/>
        public IEnumerable<IConfigurationSection> GetChildren(string key) 
        { 
            return this.GetChildren().Select(c => c.GetSection(key)); 
        }

        /// <inheritdoc/>
        public IConfigurationSection GetConfigurationSection(ConfigurationSectionType section)
        {
            return this.GetSection(section.ToString());
        }

        /// <inheritdoc/>
        public IChangeToken GetReloadToken()
        {
            return _configuration.GetReloadToken();
        }

        /// <inheritdoc/>
        public IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        private bool TryParseConfigurationValue(string key, out string value)
        {
            value = _configuration[KeyVaultUriKey];
            return value.IsNullOrWhiteSpace();
        }
    }
}
