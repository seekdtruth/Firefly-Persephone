using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Firefly.Core.Configurations;
using Environment = Firefly.Core.Configurations.Environment;
using Firefly.Utilities.Extensions;

namespace Firefly.Implementation.Configurations
{
    /// <summary>
    /// Represents an implementation of an <see cref="IFireflyConfiguration"/>
    /// </summary>
    public class FireflyConfiguration : IFireflyConfiguration
    {
        private static readonly string KeyVaultTenantIdKey = "KeyVault:TenantId";
        private static readonly string KeyVaultNameKey = "KeyVault:Name";
        private static readonly string KeyVaultUriKey = "KeyVault:Uri";

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
            loggerFactory ??= new LoggerFactory();
            _logger = loggerFactory.CreateLogger<FireflyConfiguration>();

            _keyVaultTenantId = GetRequiredValue(KeyVaultTenantIdKey);
            _KeyVaultName = GetRequiredValue(KeyVaultNameKey);
            _keyVaultUri = new Uri(GetRequiredValue(KeyVaultUriKey));

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
            return GetChildren().Select(c => c.GetSection(key));
        }

        /// <inheritdoc/>
        public IConfigurationSection GetConfigurationSection(ConfigurationSectionType section)
        {
            return GetSection(section.ToString());
        }

        /// <inheritdoc/>
        public IChangeToken GetReloadToken()
        {
            return _configuration.GetReloadToken();
        }

        public static IFireflyConfiguration CreateFireflyConfiguration(IConfiguration config)
        {
            return new FireflyConfiguration(config, loggerFactory: null);
        }

        /// <inheritdoc/>
        public IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        private bool TryParseConfigurationValue(string key, out string value)
        {
            value = _configuration[key];
            return value.IsNullOrWhitespace();
        }

        private string GetRequiredValue(string key)
        {
            if (!TryParseConfigurationValue(_configuration[key], out var value))
                throw new ArgumentNullException(key);
            return value;
        }
    }
}
