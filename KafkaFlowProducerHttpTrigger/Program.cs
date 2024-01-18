using Microsoft.Extensions.Hosting;
using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.Configuration;

namespace KafkaFlowProducerHttpTrigger
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string topicName = "ConnectionTest";
            const string producerName = "ConnectionTestConsumer";
            Action<SecurityInformation> sasl = securityInfo =>
            {
                securityInfo.SaslMechanism = SaslMechanism.Plain;
                securityInfo.SaslUsername = "your_username";
                securityInfo.SaslPassword = "your_password";
            };

            var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(s =>
            {
                s.AddKafka(
                    kafka => kafka
                        .UseConsoleLog()
                        .AddCluster(
                            cluster => cluster
                                .WithSecurityInformation(sasl)
                                .WithBrokers(new[]
                                {
                                    "localhost:6969"
                                })
                                .CreateTopicIfNotExists(topicName, 1, 1)
                                .AddProducer(
                                    producerName,
                                    producer => producer
                                        .DefaultTopic(topicName)
                                        .AddMiddlewares(m =>
                                            m.AddSerializer<JsonCoreSerializer>())
                    ))
                );
            })
            .Build();

            host.Run();
        }
    }
}