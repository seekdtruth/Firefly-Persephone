using Microsoft.Extensions.Configuration;

namespace Utilities.Configurations
{
    public class FireflyConfigurationBuilder : ConfigurationBuilder, IFireflyConfigurationBuilder
    {
        public FireflyConfigurationBuilder() { }

        public new IConfigurationRoot Build()
        {
            return base.Build();
        }
    }
}
