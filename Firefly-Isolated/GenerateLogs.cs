using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Firefly.Isolated
{
    public class GenerateLogs
    {
        private ILogger logger;
        private ILogger<GenerateLogs> loggerFromFactory;

        public GenerateLogs(ILogger logger, ILoggerFactory loggerFactory)
        {
            this.logger = logger;
            this.loggerFromFactory = loggerFactory.CreateLogger<GenerateLogs>();
        }

        [FunctionName("GenerateLogsWithDependencyInjectedLogger")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogDebug("Logging Debug");
            log.LogMetric("Logging metric", 100);
            log.LogInformation("Logging information");
            log.LogWarning("Logging warning");
            log.LogError("Logging error");
            log.LogCritical("Logging critical");

            return new OkResult();
        }


        [FunctionName("GenerateLogsWithHostLogger")]
        public IActionResult RunGenerateLogsWithHostLogger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            logger.LogDebug("Logging Debug with HostLogger");
            logger.LogMetric("Logging metric with HostLogger", 100);
            logger.LogInformation("Logging information with HostLogger");
            logger.LogWarning("Logging warning with HostLogger");
            logger.LogError("Logging error with HostLogger");
            logger.LogCritical("Logging critical with HostLogger");

            return new OkResult();
        }


        [FunctionName("GenerateLogsWithHostLoggerFactoryLogger")]
        public IActionResult GenerateLogsWithHostLoggerFactoryLogger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            loggerFromFactory.LogDebug("Logging Debug with Logger from HostFactory");
            loggerFromFactory.LogMetric("Logging metric with Logger from HostFactory", 100);
            loggerFromFactory.LogInformation("Logging information with Logger from HostFactory");
            loggerFromFactory.LogWarning("Logging warning with Logger from HostFactory");
            loggerFromFactory.LogError("Logging error with Logger from HostFactory");
            loggerFromFactory.LogCritical("Logging critical with Logger from HostFactory");

            return new OkResult();
        }

        [Function("GenerateLogsWithFunctionContext")]
        public IActionResult GenerateLogsWithFunctionContext([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, FunctionContext context)
        {
            var contextLogger = context.GetLogger<GenerateLogsWithFunctionContext>();
            contextLogger.LogInformation($"Logging with FunctionContext context.(GetLogger<{nameof(GenerateLogsWithFunctionContext)}>()");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

    }
}
