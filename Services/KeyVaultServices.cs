using System.Security.Cryptography.X509Certificates;
using Azure.Core;
using Azure.Security.KeyVault.Certificates;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Utilities.Configurations;
using Utilities.Extensions;

namespace Firefly.Services
{
    public class KeyVaultServices : IKeyVaultServices
    {
        private readonly IFireflyConfiguration configuration;
        private readonly CertificateClient certificateClient;

        private ILogger<KeyVaultServices> logger;

        public KeyVaultServices(IFireflyConfiguration configuration, ILoggerFactory? loggerFactory)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            loggerFactory = loggerFactory ?? new LoggerFactory();

            this.logger = loggerFactory.CreateLogger<KeyVaultServices>();

            this.certificateClient = new CertificateClient(configuration.KeyVaultUri, configuration.KeyVaultCredential, new CertificateClientOptions()
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(1),
                    MaxDelay = TimeSpan.FromSeconds(1),
                    MaxRetries = 1,
                    Mode = RetryMode.Exponential
                }
            });

            this.Init();
        }

        public KeyVaultServices(IServiceProvider provider) 
            : this(provider.GetRequiredService<IFireflyConfiguration>() 
                  ?? throw new ArgumentNullException(nameof(provider)),
                  provider.GetRequiredService<ILoggerFactory>())
        {
        }

        public KeyVaultServices(IFunctionsHostBuilder hostBuilder) : this(hostBuilder.GetContext().GetConfiguration())
        {
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">Thrown when no certificate name is provided</exception>
        /// <exception cref="FileNotFoundException">Thrown when a certificate isn't found for provided <paramref name="name"/></exception>
        public X509Certificate2 GetCertificate(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                name = name.TryParseNullOrEmpty(out var certName) ? certName : throw new ArgumentNullException(nameof(name));
                var response = certificateClient.GetCertificate(name, cancellationToken).Value ?? throw new FileNotFoundException($"Unable to retrieve certificate: {name}");
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
    }
}
