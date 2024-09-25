using System;
using Azure.Core;
using Microsoft.Extensions.Configuration;

namespace Firefly.Core
{
    /// <summary>
    /// Represents an <see cref="IFireflyConfiguration"/>.
    /// </summary>
    public interface IFireflyConfiguration : IConfiguration
    {
        /// <summary>
        /// The associated Key Vault Tenant ID
        /// </summary>
        string KeyVaultTenantId { get; }

        /// <summary>
        /// The Key Vault Name
        /// </summary>
        string KeyVaultName { get; }

        /// <summary>
        /// The Key Vault <see cref="Uri"/>
        /// </summary>
        Uri KeyVaultUri { get; }

        /// <summary>
        /// The Key Vault <see cref="TokenCredential"/>
        /// </summary>
        TokenCredential KeyVaultCredential { get; }

        /// <summary>
        /// The current <see cref="Environment"/>
        /// </summary>
        Environment Environment { get; }
    }
}