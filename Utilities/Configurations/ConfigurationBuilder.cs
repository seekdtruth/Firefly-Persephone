using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Utilities.Configurations
{
    public static class FireflyConfigurationHelpers
    {
        public static IConfigurationBuilder AddAppSettingsJson(this IConfigurationBuilder builder, HostBuilderContext hostingContext)
        {
            var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            builder.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: false);
            return builder;
        }

        public static IFireflyConfiguration GetConfiguration(this FunctionsHostBuilderContext context)
        {
            return new FireflyConfiguration(context.Configuration);
        }
    }
}
