using System;
using System.Collections.Generic;
using Firefly.Core;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Utilities.Configurations
{
    public class FireflyConfigurationBuilder : IFireflyConfigurationBuilder
    {
        private readonly IFireflyConfiguration _configuration;
        private readonly IConfigurationRoot _configurationRoot;
        private IConfigurationBuilder _configurationBuilder = new ConfigurationBuilder();

        public FireflyConfigurationBuilder(FunctionsHostBuilderContext context) 
        {
            var contextConfig = context.Configuration;
            _configurationRoot = Build();
            _configuration = new FireflyConfiguration(context.Configuration, loggerFactory: null);
        }

        public IDictionary<string, object> Properties => _configurationBuilder.Properties;

        public IList<IConfigurationSource> Sources => _configurationBuilder.Sources;

        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            source = source ?? throw new ArgumentNullException(nameof(source));
            _configurationBuilder.Add(source);
            return this;
        }

        public IConfigurationRoot Build()
        {
            return _configurationBuilder.Build();
        }
    }

}
