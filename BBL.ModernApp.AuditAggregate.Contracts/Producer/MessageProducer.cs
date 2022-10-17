using Confluent.Kafka;
using System.Net;

namespace BBL.ModernApp.AuditAggregate.Contracts.Producer
{
    public class MessageProducer
    {
        private readonly ProducerBuilder<string, PayloadMessage> builder;
        private readonly IProducer<string, PayloadMessage> producer;
        private readonly string topic;

        public MessageProducer(string kafkaServer, string topic)
        {
            builder = new(new ProducerConfig()
            {
                BootstrapServers = kafkaServer,
                ClientId = Dns.GetHostName(),
            });

            builder.SetValueSerializer(new PayloadMessageSerializer());

            producer = builder.Build();
            this.topic = topic;
        }

        public void Send(PayloadMessage payloadMessage)
        {
            Message<string, PayloadMessage> message = new()
            {
                Value = payloadMessage
            };

            producer.Produce(topic, message);
        }

        ~MessageProducer()
        {
            producer.Dispose();
        }
    }
}
   