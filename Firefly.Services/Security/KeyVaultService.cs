using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Firefly.Core.Configurations;
using Microsoft.Extensions.Logging;
using Firefly.Utilities.Extensions;
using Firefly.Core.Services;
using Microsoft.Extensions.Configuration;
using Firefly.Implementation.Configurations;

namespace Firefly.Services.Security
{
    /// <summary>
    /// Represents an implementation of an <see cref="IKeyVaultService"/>.
    /// </summary>
    public class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient secretClient;
        private readonly CertificateClient certificateClient;

        private readonly Dictionary<string, KeyVaultSecret> retrievedSecrets = new Dictionary<string, KeyVaultSecret>();
        private readonly Dictionary<string, X509Certificate2> certificateCollection = new Dictionary<string, X509Certificate2>();

        private readonly ILogger<KeyVaultService> logger;

        public static IKeyVaultService CreateInstance(IConfiguration configuration, ILoggerFactory? loggerFactory)
        {
            return new KeyVaultService(new FireflyConfiguration(configuration, loggerFactory), loggerFactory);
        }

        /// <summary>
        /// Creates an instance of a <see cref="KeyVaultService"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
        public KeyVaultService(IFireflyConfiguration configuration, ILoggerFactory? loggerFactory)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            loggerFactory ??= new LoggerFactory();

            logger = loggerFactory.CreateLogger<KeyVaultService>();

            secretClient = new SecretClient(configuration.KeyVaultUri, configuration.KeyVaultCredential, new SecretClientOptions()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(1),
                    MaxRetries = 1,
                    Mode = RetryMode.Exponential
                }
            });

            certificateClient = new CertificateClient(configuration.KeyVaultUri, configuration.KeyVaultCredential, new CertificateClientOptions()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(1),
                    MaxRetries = 1,
                    Mode = RetryMode.Exponential
                }
            });
        }

        /// <inheritdoc />
        public KeyVaultSecret GetSecret(string secretKey, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (TryGetSecret(secretKey, out var secret)) return secret;

                var response = secretClient.GetSecret(secretKey, cancellationToken: cancellationToken) 
                    ?? throw new ApplicationException($"Unable to retrieve secret: {secretKey}");
                retrievedSecrets.Add(secretKey, response.Value);

                return response.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when no secret name is provided</exception>
        /// <exception cref="FileNotFoundException">Thrown when a secret isn't found for provided <paramref name="secretKey"/></exception>
        public async Task<KeyVaultSecret> GetSecretAsync(string secretKey, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (TryGetSecret(secretKey, out var secret)) return secret;

                var response = await secretClient.GetSecretAsync(secretKey, cancellationToken: cancellationToken).ConfigureAwait(false) 
                    ?? throw new ApplicationException($"Unable to retrieve secret: {secretKey}");
                retrievedSecrets.Add(secretKey, response.Value);

                return response.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when no certificate name is provided</exception>
        /// <exception cref="FileNotFoundException">Thrown when a certificate isn't found for provided <paramref name="certificateName"/></exception>
        public X509Certificate2 GetCertificate(string certificateName, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (TryGetCertificate(certificateName, out var certificate)) return certificate;

                var response = certificateClient.GetCertificate(certificateName, cancellationToken).Value ?? throw new FileNotFoundException($"Unable to retrieve certificate: {certificateName}");
                var bytes = response.Cer.Any() ? response.Cer : throw new FileNotFoundException($"Certificate {certificateName} was empty");
                var cert = new X509Certificate2(bytes);

                certificateCollection.Add(certificateName, cert);

                return cert;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        /// <inheritdoc cref="GetCertificate(string, CancellationToken)"/>
        public async Task<X509Certificate2> GetCertificateAsnc(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                name = name.TryParseNullOrEmpty(out var certName) ? certName : throw new ArgumentNullException(nameof(name));
                var task = await certificateClient.GetCertificateAsync(name, cancellationToken).ConfigureAwait(false);
                var response = task.Value ?? throw new FileNotFoundException($"Unable to retrieve certificate: {name}");
                var bytes = response.Cer.Any() ? response.Cer : throw new FileNotFoundException($"Certificate {name} was empty");
                var cert = new X509Certificate2(bytes);
                return cert;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
        }

        private bool TryGetSecret(string secretName, out KeyVaultSecret secret)
        {
            secretName = secretName.TryParseNullOrEmpty(out var name) ? name : throw new ArgumentNullException(nameof(secretName));
            secret = retrievedSecrets.ContainsKey(secretName) ? retrievedSecrets[secretName] : default;
            return retrievedSecrets.ContainsKey(secretName);
        }

        private bool TryGetCertificate(string certificateName, out X509Certificate2 certificate)
        {
            certificateName = certificateName.TryParseNullOrEmpty(out var name) ? name : throw new ArgumentNullException(nameof(certificateName));
            certificate = certificateCollection.ContainsKey(certificateName) ? certificateCollection[certificateName] : new X509Certificate2();
            return certificateCollection.ContainsKey(certificateName);
        }
    }
}
