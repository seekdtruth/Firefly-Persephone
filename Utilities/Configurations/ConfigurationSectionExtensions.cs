using Microsoft.Extensions.Configuration;

namespace Utilities.Configurations
{
    public static class ConfigurationSectionExtensions
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
