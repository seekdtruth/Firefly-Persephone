using System;
using System.Threading.Tasks;
using Firefly.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Firefly.Security
{
    public class TestGetCertificate
    {
        private readonly ILogger<TestGetCertificate> _logger;
        private readonly IKeyVaultService _keyVaultService;

        public TestGetCertificate(ILogger<TestGetCertificate> logger, IKeyVaultService service)
        {
            _logger = logger;
            _keyVaultService = service;
        }

        [Function("TestGetCertificate")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation($"Entering the {nameof(TestGetCertificate)} method.");

                Task.Run(async () =>
                {
                    var certificate2 = await _keyVaultService.GetCertificateAsync("Persephone-Eavesdown-Cert").ConfigureAwait(false);

                    _logger.LogInformation($"TestGetCertificate retrieved with version:{certificate2.Version}");
                });
                return new OkObjectResult("Welcome to Azure Functions!");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            finally
            {
                _logger.LogInformation($"Leaving the {nameof(TestGetCertificate)} method.");
            }
        }
    }
}
