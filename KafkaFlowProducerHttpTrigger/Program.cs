using Microsoft.Extensions.Hosting;
using KafkaFlow;
using KafkaFlow.Serializer;

namespace KafkaFlowProducerHttpTrigger
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string topicName = "Order";
            const string producerName = "CustomerOrderProducer";

            var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices(s =>
            {
                s.AddKafka(
                    kafka => kafka
                        .UseConsoleLog()
                        .AddCluster(
                            cluster => cluster
                                .WithBrokers(new[]
                                {
                                    "kafka.confluent.svc.cluster.local:9071"
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