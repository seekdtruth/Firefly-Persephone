using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Firefly_Isolated
{
    public class GenerateLogs
    {
        private ILogger logger;

        public GenerateLogs(ILogger logger)
        {
            this.logger = logger;
        }

        [FunctionName("GenerateLogsWithRequestLogger")]
        public IActionResult RunGenerateLogsWithRequestLogger(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogDebug("Logging Debug");
            log.LogMetric("Logging metric", 100);
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
            logger.LogDebug("Logging Debug");
            logger.LogMetric("Logging metric", 100);
            logger.LogWarning("Logging warning");
            logger.LogError("Logging error");
            logger.LogCritical("Logging critical");

            return new OkResult();
        }

    }
}
