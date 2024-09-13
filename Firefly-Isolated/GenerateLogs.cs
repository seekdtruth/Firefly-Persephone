using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Firefly.Isolated
{
    public class GenerateLogs
    {
        private ILogger<GenerateLogs> loggerFromDI;

        public GenerateLogs(ILogger<GenerateLogs> logger)
        {
            this.loggerFromDI = logger;
        }

        [FunctionName(nameof(GenerateLogsWithRequestLogger))]
        public IActionResult GenerateLogsWithRequestLogger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogDebug("Logging Debug from Request Logger");
            log.LogMetric("Logging metric from Request Logger", 100);
            log.LogInformation("Logging information from Request Logger");
            log.LogWarning("Logging warning from Request Logger");
            log.LogError("Logging error from Request Logger");
            log.LogCritical("Logging critical from Request Logger");

            return new OkResult();
        }

        [FunctionName(nameof(GenerateLogsWithDependencyInjectedLogger))]
        public IActionResult GenerateLogsWithDependencyInjectedLogger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, FunctionContext context)
        {
            loggerFromDI.LogDebug("Logging Debug with Logger from GenerateLogsWithDependencyInjectedLogger");
            loggerFromDI.LogMetric("Logging metric with Logger from GenerateLogsWithDependencyInjectedLogger", 100);
            loggerFromDI.LogInformation("Logging information with Logger from GenerateLogsWithDependencyInjectedLogger");
            loggerFromDI.LogWarning("Logging warning with Logger from GenerateLogsWithDependencyInjectedLogger");
            loggerFromDI.LogError("Logging error with Logger from GenerateLogsWithDependencyInjectedLogger");
            loggerFromDI.LogCritical("Logging critical with Logger from GenerateLogsWithDependencyInjectedLogger");

            return new OkResult();
        }

        [Function(nameof(GenerateLogsWithFunctionContext))]
        public IActionResult GenerateLogsWithFunctionContext([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, FunctionContext context)
        {
            var contextLogger = context.GetLogger<GenerateLogs>();
            contextLogger.LogInformation($"Logging with FunctionContext context.(GetLogger<{nameof(GenerateLogs)}>()");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

    }
}
