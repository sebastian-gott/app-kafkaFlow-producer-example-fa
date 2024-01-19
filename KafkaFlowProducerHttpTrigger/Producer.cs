using System.Net;
using System.Text;
using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlowProducerHttpTrigger.Extentions;
using KafkaFlowProducerHttpTrigger.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Conf = Confluent.Kafka;

namespace KafkaFlowProducerHttpTrigger
{
    public class Producer
    {
        private readonly ILogger _logger;
        private readonly IProducerAccessor _producer;

        public Producer(ILoggerFactory loggerFactory, IProducerAccessor producer)
        {
            _logger = loggerFactory.CreateLogger<Producer>();
            _producer = producer;
        }

        [Function("KafkaFlowProduceCustomerOrder")]
        public async Task<HttpResponseData> ProduceOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "trigger/TestMessage/")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("Function: {functionApp} processed a request", "KafkaFlowProduceTestMessage");

                ValidationWrapper<TestMessage> httpResponseBody = await req.GetBody<TestMessage>();

                if (!httpResponseBody.IsValid)
                {
                    var validationResults = httpResponseBody.ValidationResults.Select(s => s.ErrorMessage).ToArray();

                    _logger.LogWarning("Some parameters are missing or are invalid: {validationResults}", (object)validationResults);

                    var response = req.CreateResponse(HttpStatusCode.BadRequest);
                    await response.WriteStringAsync($"Some parameters are missing or are invalid: {string.Join(", ", validationResults)}");

                    return response;
                }

                await CreateTestMessage(httpResponseBody.Value);
                _logger.LogInformation("{type} processed successfully", "TestMessage");
                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while processing the request: {exception}", ex);

                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync($"An error occurred while processing the request: {ex}");

                return response;
            }
        }

        public async Task CreateTestMessage(TestMessage message) =>
            await _producer["CustomerOrderProducer"]
                .ProduceAsync(
                    "TopicName",
                    Guid.NewGuid().ToString(),
                    message,
                    headers: new MessageHeaders(new Conf.Headers { new Conf.Header("messageType", Encoding.UTF8.GetBytes("TestMessage")) }
                ));
    }
}
