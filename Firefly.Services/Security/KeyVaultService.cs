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

        public KeyVaultService(IConfiguration configuration, ILoggerFactory? loggerFactory) : this(
            new FireflyConfiguration(configuration, loggerFactory), loggerFactory) {}

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
        /// <exception cref="ApplicationException">Thrown when no secret is retrieved</exception>
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
        public async Task<X509Certificate2> GetCertificateAsync(string certificateName, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                if (TryGetCertificate(certificateName, out var certificate)) return certificate;

                logger.LogInformation("Getting Certificate");
                certificateName = !certificateName.IsNullOrWhitespace() ? certificateName : throw new ArgumentNullException(nameof(certificateName));

                var task = await certificateClient.GetCertificateAsync(certificateName, cancellationToken).ConfigureAwait(false);

                var keyVaultCertificateWithPolicy = task.Value ?? throw new FileNotFoundException($"Unable to retrieve certificate: {certificateName}");

                var bytes = keyVaultCertificateWithPolicy.Cer.Any()
                    ? keyVaultCertificateWithPolicy.Cer
                    : throw new FileNotFoundException($"Certificate {certificateName} was empty");

                var cert = new X509Certificate2(bytes) { FriendlyName = keyVaultCertificateWithPolicy.Name };

                return cert;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                throw;
            }
            finally
            {
                logger.LogInformation($"Retrieval complete");
            }
        }

        public async Task<X509Certificate2> DownloadCertificateAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                logger.LogInformation($"Entering method {nameof(DownloadCertificateAsync)}");

                name = !name.IsNullOrWhitespace() ? name : throw new ArgumentNullException(nameof(name));

                var task = await certificateClient.DownloadCertificateAsync(name).ConfigureAwait(false);

                if (task.GetRawResponse().IsError)
                {
                    logger.LogError($"Response had error. {task.GetRawResponse().Status}: {task.GetRawResponse().ReasonPhrase}");
                    throw new FileNotFoundException($"Response had error. {task.GetRawResponse().Status}: {task.GetRawResponse().ReasonPhrase}");
                }

                if (!task.HasValue)
                {
                    logger.LogError("Response has no value");
                    throw new FileNotFoundException("No response was received");
                }

                return task.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
            finally
            {
                logger.LogInformation($"Exiting method {nameof(DownloadCertificateAsync)}");
            }
        }

        private bool TryGetSecret(string secretName, out KeyVaultSecret secret)
        {
            secretName = !secretName.IsNullOrWhitespace() ? secretName : throw new ArgumentNullException(nameof(secretName));
            secret = retrievedSecrets.ContainsKey(secretName) ? retrievedSecrets[secretName] : default;
            return retrievedSecrets.ContainsKey(secretName);
        }

        private bool TryGetCertificate(string certificateName, out X509Certificate2 certificate)
        {
            certificateName = !certificateName.IsNullOrWhitespace() ? certificateName : throw new ArgumentNullException(nameof(certificateName));
            certificate = certificateCollection.ContainsKey(certificateName) ? certificateCollection[certificateName] : new X509Certificate2();
            return certificateCollection.ContainsKey(certificateName);
        }
    }
}
