using System;
using System.Collections.Generic;
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
        private const string KeyVaultTenantIdKey = "KeyVaultTenantId";
        private const string KeyVaultNameKey = "KeyVaultName";
        private const string KeyVaultUriKey = "KeyVaultUri";

        private readonly IConfiguration _configuration;
        private readonly ILogger<FireflyConfiguration> _logger;

        /// <summary>
        /// Creates an instance of a <see cref="FireflyConfiguration"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null</exception>
        public FireflyConfiguration(IConfiguration configuration, ILoggerFactory? loggerFactory)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Environment = EnvironmentExtensions.GetEnvironment();
            loggerFactory ??= new LoggerFactory();
            _logger = loggerFactory.CreateLogger<FireflyConfiguration>();
            _logger.LogInformation("Settings up Configuration and KeyVault credentials");

            KeyVaultTenantId = GetRequiredValue(KeyVaultTenantIdKey);
            KeyVaultName = GetRequiredValue(KeyVaultNameKey);
            KeyVaultUri = new Uri(GetRequiredValue(KeyVaultUriKey));

            KeyVaultCredential = string.IsNullOrWhiteSpace(KeyVaultTenantId)
                ? new DefaultAzureCredential()
                : new DefaultAzureCredential(new DefaultAzureCredentialOptions { VisualStudioTenantId = KeyVaultTenantId });
        }

        /// <inheritdoc/>
        public string? this[string key]
        {
            get => _configuration[key];
            set => _configuration[key] = value;
        }

        /// <inheritdoc/>
        public string KeyVaultTenantId { get; }

        /// <inheritdoc/>
        public string KeyVaultName { get; }

        /// <inheritdoc/>
        public Uri KeyVaultUri { get; }

        /// <inheritdoc/>
        public TokenCredential KeyVaultCredential { get; }

        /// <inheritdoc/>
        public Environment Environment { get; }

        /// <inheritdoc/>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return _configuration.GetChildren();
        }

        /// <inheritdoc/>
        public IChangeToken GetReloadToken()
        {
            return _configuration.GetReloadToken();
        }

        /// <inheritdoc cref="CreateFireflyConfiguration(IConfiguration, ILoggerFactory)"/>
        public static IFireflyConfiguration CreateFireflyConfiguration(IConfiguration config) =>
            CreateFireflyConfiguration(config, new LoggerFactory());

        /// <summary>
        /// Create an instance of an <see cref="IFireflyConfiguration"/>
        /// </summary>
        /// <param name="config">An <see cref="IConfiguration"/> holding required configuration values.</param>
        /// <param name="factory">The logger factory</param>
        /// <returns>An instance of an <see cref="IFireflyConfiguration"/></returns>
        public static IFireflyConfiguration CreateFireflyConfiguration(IConfiguration config, ILoggerFactory factory)
        {
            return new FireflyConfiguration(config, factory);
        }

        /// <inheritdoc/>
        public IConfigurationSection GetSection(string key)
        {
            return _configuration.GetSection(key);
        }

        private bool TryParseConfigurationValue(string key, out string value)
        {
            value = _configuration[key];
            return !value.IsNullOrWhitespace();
        }

        private string GetRequiredValue(string key)
        {
            try
            {
                if (!TryParseConfigurationValue(key, out var value))
                    throw new ArgumentNullException(key);
                return value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
