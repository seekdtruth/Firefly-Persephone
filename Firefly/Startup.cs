using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Firefly.Services;

[assembly: FunctionsStartup(typeof(Firefly.Dotnet.Startup))]

namespace Firefly.Dotnet
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //var queueNames = await GetQueueNames();
            builder.Services.AddLogging();
            builder.Services.TryAddSingleton<IKeyVaultServices>((provider) => new KeyVaultServices(provider));
        }
    }
}
