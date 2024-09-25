using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Firefly.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities.Configurations;
using Firefly.Core;

[assembly: FunctionsStartup(typeof(Firefly.Dotnet.Startup))]

namespace Firefly.Dotnet
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var queueNames = await GetQueueNames();
            builder.Services.AddLogging();
            builder.Services.TryAddSingleton<IKeyVaultService>((provider) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var factory = provider.GetRequiredService<ILoggerFactory>();

                return new KeyVaultService(new FireflyConfiguration(config, factory), factory);
            });
        }

        private static async Task<List<string>> GetQueueNames()
        {
            // Query the available queues for the Service Bus namespace.
            var adminClient = new ServiceBusAdministrationClient
                ("<your_namespace>.servicebus.windows.net", new DefaultAzureCredential());
            var queueNames = new List<string>();

            // Because the result is async, the queue names need to be captured
            // to a standard list to avoid async calls when registering. Failure to
            // do so results in an error with the services collection.
            await foreach (QueueProperties queue in adminClient.GetQueuesAsync())
            {
                queueNames.Add(queue.Name);
            }

            return queueNames;
        }
    }
}
