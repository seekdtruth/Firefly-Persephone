using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Firefly.Isolated
{
    public class GenerateLogsWithFunctionContext
    {
        private readonly ILogger<GenerateLogsWithFunctionContext> _logger;

        public GenerateLogsWithFunctionContext(ILogger<GenerateLogsWithFunctionContext> logger)
        {
            _logger = logger;
        }

        [Function("GenerateLogsWithFunctionContext")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, FunctionContext context)
        {
            _logger.LogInformation("Logging with Ctor DI Logger - C# HTTP trigger function processed a request.");
            var contextLogger = context.GetLogger<GenerateLogsWithFunctionContext>();
            contextLogger.LogInformation($"Logging with FunctionContext context.(GetLogger<{nameof(GenerateLogsWithFunctionContext)}>()");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
