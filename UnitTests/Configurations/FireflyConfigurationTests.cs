using Firefly.Core.Configurations;
using Firefly.Implementation.Configurations;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.Configurations
{
    public class FireflyConfigurationTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IConfiguration> mockConfiguration;

        public FireflyConfigurationTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockConfiguration = this.mockRepository.Create<IConfiguration>();
        }

        private IFireflyConfiguration CreateFireflyConfiguration()
        {
            return FireflyConfiguration.CreateFireflyConfiguration(this.mockConfiguration.Object);
        }

        [Fact]
        public void GetChildren_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fireflyConfiguration = this.CreateFireflyConfiguration();

            // Act
            var result = fireflyConfiguration.GetChildren();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetChildren_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var fireflyConfiguration = this.CreateFireflyConfiguration();
            string key = String.Empty;

            // Act
            var result = fireflyConfiguration.GetChildren();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetReloadToken_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fireflyConfiguration = this.CreateFireflyConfiguration();

            // Act
            var result = fireflyConfiguration.GetReloadToken();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void GetSection_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fireflyConfiguration = this.CreateFireflyConfiguration();
            string key = null;

            // Act
            var result = fireflyConfiguration.GetSection(
                key);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void TryParseNullOrEmpty_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var fireflyConfiguration = this.CreateFireflyConfiguration();
            string value = null;

            // Act
            //var result = fireflyConfiguration.TryParseNullOrEmpty(
            //    out value);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
