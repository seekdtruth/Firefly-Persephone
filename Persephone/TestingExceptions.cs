using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PersephoneAlerting
{
    public static class TestingExceptions
    {
        [FunctionName("TestException")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("C# HTTP trigger function is processing a request.");

                string message = req.Query["message"];

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                message = message ?? data?.message;

                log.LogInformation($"Message: {message}");

                throw new DivideByZeroException(message ?? "");
            }
            catch (Exception ex)
            {

                log.LogError(ex, ex.Message);
                throw;
            }

            finally
            {
                log.LogInformation("C# HTTP trigger function finished processing a request.");
            }
        }
    }
}
