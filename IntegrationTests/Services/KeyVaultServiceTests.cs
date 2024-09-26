using Firefly.Core.Configurations;
using Firefly.Core.Services;
using Firefly.Services;
using IntegrationTests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests.Services
{
    public class KeyVaultServiceTests
    {
        IFireflyConfiguration Configuration { get; }
        ILoggerFactory LoggerFactory { get; }
        IKeyVaultService Service { get; }

        public KeyVaultServiceTests()
        {
            Configuration = new TestFireflyConfiguration();
            LoggerFactory = new LoggerFactory();
            Service = new KeyVaultService(Configuration, LoggerFactory);
        }

        [Fact]
        public void GetSecret_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = new KeyVaultService(Configuration, LoggerFactory);

            string secretKey = null;
            CancellationToken cancellationToken = default;

            // Act
            var result = service.GetSecret(
                secretKey,
                cancellationToken);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetSecretAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string secretKey = null;
            CancellationToken cancellationToken = default;

            // Act
            var result = await Service.GetSecretAsync(
                secretKey,
                cancellationToken);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public void GetCertificate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string certificateName = null;
            CancellationToken cancellationToken = default;

            // Act
            var result = Service.GetCertificate(
                certificateName,
                cancellationToken);

            // Assert
            Assert.True(false);
        }

        [Fact]
        public async Task GetCertificateAsnc_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            string name = null;
            CancellationToken cancellationToken = default;

            // Act
            var result = await Service.GetCertificateAsnc(
                name,
                cancellationToken);

            // Assert
            Assert.True(false);
        }
    }
}
