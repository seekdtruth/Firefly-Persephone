using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Amqp;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EventHubs
{
    public class EventHubTest
    {
        private readonly ILogger<EventHubTest> _logger;

        public EventHubTest(ILogger<EventHubTest> logger)
        {
            _logger = logger;
        }

        //[Function(nameof(EventHubTest))]
        public void Run([EventHubTrigger("fireflyhb", Connection = "EH_CONNECTION_STRING", IsBatched = false)] EventData @event)
        {
            _logger.LogInformation("Event Body: {body}", @event.Body);
            _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
        }

        [Function("BatchedEventHubTest")]
        public void RunBatch(
            [EventHubTrigger("fireflyhb", Connection = "EH_CONNECTION_STRING", IsBatched = true)]
            EventData[] events)
        {
            _logger.LogInformation($"Number of events in batch: {events.Length}");

            foreach (var @event in events)
            {
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            }
        }
    }
}
