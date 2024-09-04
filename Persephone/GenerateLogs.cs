using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace PersephoneAlerting
{
    public static class GenerateLogs
    {
        [FunctionName("GenerateLogs")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogDebug("Logging Debug");
            log.LogInformation("Logging information");
            log.LogMetric("Logging metric", 100);
            log.LogWarning("Logging warning");
            log.LogError("Logging error");
            log.LogCritical("Logging critical");

            return new OkResult();
        }
    }
}
