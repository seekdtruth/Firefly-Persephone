using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firefly.Core.Configurations;
using Firefly.Implementation;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Helpers
{
    internal class Helpers
    {
        public static IFireflyConfiguration CreateConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();
            builder.Build();
            var root = new ConfigurationRoot(new List<IConfigurationProvider>()
            {

            });

            return new FireflyConfiguration(builder.Build());
        }
    }
}
