using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BBL.ModernApp.AuditAggregate.Contracts
{
    internal class PayloadJsonConverter : JsonConverter<PayloadMessage>
    {
        public override PayloadMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            PayloadMessage payloadMessage = new();
            PropertyInfo[] payloadMessageProps = payloadMessage.GetType().GetProperties();
            string? propertyName = null;

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject) break;
                if (reader.TokenType == JsonTokenType.PropertyName) { propertyName = reader.GetString(); continue; }

                PropertyInfo? prop = payloadMessageProps.SingleOrDefault(p => p.Name == propertyName);

                if (prop == null) continue;

                object? propValue = null;

                if (prop.PropertyType.Name == typeof(string).Name)
                {
                    propValue = reader.GetString();
                }
                else if (prop.PropertyType.Name == typeof(DateTime).Name)
                {
                    propValue = reader.GetDateTime();
                }
                else if (prop.PropertyType.Name == typeof(bool).Name)
                {
                    propValue = reader.GetBoolean();
                }
                else if (prop.PropertyType.Name == typeof(int?).Name)
                {
                    propValue = reader.GetInt32();
                }

                prop.SetValue(payloadMessage, propValue);

                propertyName = null;
            }

            return payloadMessage;
        }

        public override void Write(Utf8JsonWriter writer, PayloadMessage value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
