using Confluent.Kafka;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BBL.ModernApp.AuditAggregate.Client
{
    public class PayloadMessage 
    {
        public string? SessionID { get; set; }
        public DateTime OperationDateTime { get; set; }
        public string OperationName { get; set; } = null!;
        public string CustomerID { get; set; } = null!;
        public string Channel { get; set; } = null!;
        public string? SubChannel { get; set; }
        public string? DeviceID { get; set; }
        public string? IPAddress { get; set; }
        public bool Succeeded { get; set; }
        public string? ErrorCode { get; set; }
        public string? Info1 { get; set; }
        public string? Info2 { get; set; }
        public string? Info3 { get; set; }
        public string? Info4 { get; set; }
        public string? Info5 { get; set; }
        public string? Info6 { get; set; }
        public string? Info7 { get; set; }
        public string? Info8 { get; set; }
        public string? Info9 { get; set; }
        public string? Info10 { get; set; }
        public string? Keyword { get; set; }
        public string? DisplayMessage { get; set; } = null!;

        
        public PayloadMessage() { }

        public static PayloadMessage New(
            string operationName
            , DateTime operationDateTime
            , string channel
            , string customerId
            , bool isSuccess
            , string? sessionId = null
            , string? subChannel = null
            , string? deviceId = null
            , string? ipAddress = null
            , string? errorCode = null
            , string? keyword = null
            , string? displayMessage = null
            , string[]? infos = null)
        {
            PayloadMessage payloadMessage = new()
            {
                OperationName = operationName
                , OperationDateTime = operationDateTime
                , Channel = channel
                , CustomerID = customerId
                , Succeeded = isSuccess
                , SessionID = sessionId
                , SubChannel = subChannel
                , DeviceID = deviceId
                , IPAddress = ipAddress
                , ErrorCode = errorCode
                , Keyword = keyword
                , DisplayMessage = displayMessage
            };

            if (infos != null)
            {
                PropertyInfo[] propInfos = payloadMessage
                    .GetType()
                    .GetProperties()
                    .Where(p => p.Name.StartsWith("Info"))
                    .Take(infos.Length)
                    .ToArray();

                for (var i = 0; i < infos.Length; i++)
                {
                    propInfos[i].SetValue(payloadMessage, infos[i]);
                }
            }

            return payloadMessage;
        }
    }
}
