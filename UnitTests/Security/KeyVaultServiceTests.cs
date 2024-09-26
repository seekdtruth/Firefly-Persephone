using Firefly.Core.Configurations;
using Firefly.Services.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.Security
{
    public class KeyVaultServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IFireflyConfiguration> mockFireflyConfiguration;

        public KeyVaultServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockFireflyConfiguration = this.mockRepository.Create<IFireflyConfiguration>();
        }

        private KeyVaultService CreateService()
        {
            return new KeyVaultService(
                this.mockFireflyConfiguration.Object,
                TODO);
        }

        [Fact]
        public void CreateInstance_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            IConfiguration configuration = null;
            ILoggerFactory? loggerFactory = null;

            // Act
            var result = service.CreateInstance(
                configuration,
                loggerFactory);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetSecret_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string secretKey = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = service.GetSecret(
                secretKey,
                cancellationToken);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetSecretAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string secretKey = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await service.GetSecretAsync(
                secretKey,
                cancellationToken);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetCertificate_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string certificateName = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = service.GetCertificate(
                certificateName,
                cancellationToken);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task GetCertificateAsnc_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            string name = null;
            CancellationToken cancellationToken = default(global::System.Threading.CancellationToken);

            // Act
            var result = await service.GetCertificateAsnc(
                name,
                cancellationToken);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
