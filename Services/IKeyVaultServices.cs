using System.Security.Cryptography.X509Certificates;

namespace Firefly.Services
{
    public interface IKeyVaultServices
    {
        /// <summary>
        /// Retrieves a <see cref="X509Certificate2" />
        /// </summary>
        /// <param name="name">Name of certificate to retrieve</param>
        /// <param name="cancellationToken">(Optional) Cancellation token</param>
        /// <returns>Requested <see cref="X509Certificate2" /></returns>
        X509Certificate2 GetCertificate(string name, CancellationToken cancellationToken = new CancellationToken());

        /// <inheritdoc cref="GetCertificate(string, CancellationToken)"/>
        Task<X509Certificate2> GetCertificateAsnc(string name, CancellationToken cancellationToken = new CancellationToken());
    }
}
