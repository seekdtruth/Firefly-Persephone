using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace PersephoneAlerting
{
    public class Heartbeat
    {
        [FunctionName("Heartbeat")]
        public static void Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Heartbeat function executed at: {DateTime.Now}");
        }
    }
}
