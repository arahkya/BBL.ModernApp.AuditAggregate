using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace BBL.ModernApp.AuditAggregate.Client
{
    public class PayloadMessageSerializer : ISerializer<PayloadMessage>, IDeserializer<PayloadMessage>
    {
        public byte[] Serialize(PayloadMessage data, SerializationContext context)
        {
            string json = JsonSerializer.Serialize(data);

            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            return jsonBytes;
        }

        public PayloadMessage Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            string json = Encoding.UTF8.GetString(data);

            PayloadMessage? payloadMessage = JsonSerializer.Deserialize<PayloadMessage>(json);

            if (payloadMessage == null)
            {
                throw new Exception("Cannot parse json to PayloadMessage");
            }

            return payloadMessage;
        }
    }
}
