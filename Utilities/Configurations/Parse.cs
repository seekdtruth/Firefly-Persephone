using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Configurations
{
    public static class ParseConfigurations
    {
        public static ConfigurationSectionType ParseConfigurationSection(string configurationName)
        {
            return Enum.TryParse<ConfigurationSectionType>(configurationName, out var section)
                ? section : throw new ArgumentOutOfRangeException($"{configurationName} is not a valid configuration section");
        }
    }
}
