using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Firefly_Isolated
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
            context.GetLogger<GenerateLogsWithFunctionContext>().LogInformation($"Logging with FunctionContext context.(GetLogger<{nameof(GenerateLogsWithFunctionContext)}>()");
            context.GetLogger(nameof(GenerateLogsWithFunctionContext)).LogInformation($"Logging with FunctionContext context.(GetLogger({nameof(GenerateLogsWithFunctionContext)})");

            context.GetLogger(nameof(GenerateLogsWithFunctionContext)).LogInformation($"Logging with FunctionContext context.(GetLogger({nameof(GenerateLogsWithFunctionContext)}, context.InvocationId)", context.InvocationId);
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
