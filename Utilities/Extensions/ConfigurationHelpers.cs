using Firefly.Core;
using Microsoft.Extensions.Configuration;
using Utilities.Configurations;

namespace Utilities.Extensions
{
    public static class ConfigurationHelpers
    {
        public static IConfigurationSection GetSection(this IFireflyConfiguration configuration, ConfigurationSectionType configurationSections)
        {
            return configurationSections switch
            {
                ConfigurationSectionType.Firefly => configuration.GetSection("Firefly"),
                ConfigurationSectionType.Persephone => configuration.GetSection("Persephone"),
                _ => configuration.GetSection("Defaults"),
            };
        }
    }
}
