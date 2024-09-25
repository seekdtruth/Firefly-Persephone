using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;

namespace Firefly.Core
{
    /// <summary>
    /// Represents an <see cref="IKeyVaultService"/>
    /// </summary>
    public interface IKeyVaultService
    {
        /// <summary>
        /// Retrieves a <see cref="KeyVaultSecret"/>
        /// </summary>
        /// <param name="secretKey">Name of the <see cref="KeyVaultSecret"/></param>
        /// <returns>Requested <see cref="KeyVaultSecret"/></returns>
        KeyVaultSecret GetSecret(string secretKey, CancellationToken cancellationToken = new CancellationToken());

        /// <inheritdoc cref="GetSecret(string, CancellationToken)"/>s
        Task<KeyVaultSecret> GetSecretAsync(string secretKey, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Retrieves a <see cref="X509Certificate2" />
        /// </summary>
        /// <param name="name">Name of certificate to retrieve</param>
        /// <returns>Requested <see cref="X509Certificate2" /></returns>
        X509Certificate2 GetCertificate(string name, CancellationToken cancellationToken = new CancellationToken());

        /// <inheritdoc cref="GetCertificate(string, CancellationToken)"/>
        Task<X509Certificate2> GetCertificateAsnc(string name, CancellationToken cancellationToken = new CancellationToken());
    }
}
