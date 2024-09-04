using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

internal class Program
{
    private static void Main(string[] args)
    {
        //var host = new HostBuilder()
        //.ConfigureFunctionsWebApplication()
        //.ConfigureServices(services =>
        //{
        //    services.AddApplicationInsightsTelemetryWorkerService();
        //    services.ConfigureFunctionsApplicationInsights();
        //})
        //.Build();
        //host.Run();

        new HostBuilder().ConfigureFunctionsWebApplication()
            .ConfigureLogging(logBuilder =>
            {
                // .NET defaults the minimum log level to Information, so setting this to Debug will allow any Debug or higher logs
                // to flow from the worker to the host. At that point, the host.json log level settings will be applied.
                logBuilder.SetMinimumLevel(LogLevel.Information);
            })
            .ConfigureServices(delegate (IServiceCollection services)
            {
                services.AddApplicationInsightsTelemetryWorkerService();
                services.ConfigureFunctionsApplicationInsights();
                services.Configure(delegate (LoggerFilterOptions options)
                {
                    LoggerFilterRule loggerFilterRule = options.Rules.FirstOrDefault((LoggerFilterRule rule) => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");
                    if (loggerFilterRule != null)
                    {
                        options.Rules.Remove(loggerFilterRule);
                    }
                    options.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
                });
                services.Configure(delegate (KestrelServerOptions options)
                {
                    options.AllowSynchronousIO = true;
                });
            }).Build()
            .Run();
    }
}