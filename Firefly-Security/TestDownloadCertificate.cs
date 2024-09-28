using System;
using System.Threading.Tasks;
using Firefly.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Firefly.Security
{
    public class TestDownloadCertificate
    {
        private readonly ILogger<TestDownloadCertificate> _logger;
        private readonly IKeyVaultService _keyVaultService;

        public TestDownloadCertificate(ILogger<TestDownloadCertificate> logger, IKeyVaultService service)
        {
            _logger = logger;

            _keyVaultService = service;
        }

        [Function("TestDownloadCertificate")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            try
            {
                _logger.LogInformation($"Entering the {nameof(TestDownloadCertificate)} method.");

                Task.Run(async () =>
                    {
                        var certificate2 = await _keyVaultService.DownloadCertificateAsync("Persephone-Eavesdown-Cert");

                        _logger.LogInformation($"TestDownloadCertificate retrieved with version:{certificate2.Version}");
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
                _logger.LogInformation($"Leaving the { nameof(TestDownloadCertificate)} method.");
            }
        }
    }
}
