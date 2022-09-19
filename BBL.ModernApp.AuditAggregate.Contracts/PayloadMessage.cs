using System.Reflection;
using System.Text.Json.Serialization;

namespace BBL.ModernApp.AuditAggregate.Contracts
{
    public class PayloadMessage
    {
        public string? SessionID { get; internal set; }
        public DateTime OperationDateTime { get; internal set; }
        public string OperationName { get; internal set; } = null!;
        public string CustomerID { get; internal set; } = null!;
        public string Channel { get; internal set; } = null!;
        public string? SubChannel { get; internal set; }
        public int? DeviceID { get; internal set; }
        public string? IPAddress { get; internal set; }
        public bool Succeeded { get; internal set; }
        public string? ErrorCode { get; internal set; }
        public string? Info1 { get; internal set; }
        public string? Info2 { get; internal set; }
        public string? Info3 { get; internal set; }
        public string? Info4 { get; internal set; }
        public string? Info5 { get; internal set; }
        public string? Info6 { get; internal set; }
        public string? Info7 { get; internal set; }
        public string? Info8 { get; internal set; }
        public string? Info9 { get; internal set; }
        public string? Info10 { get; internal set; }
        public string? Keyword { get; internal set; }
        public string? DisplayMessage { get; internal set; } = null!;

        [JsonConstructor]
        internal PayloadMessage() { }

        public static PayloadMessage New(
            string operationName
            , DateTime operationDateTime
            , string channel
            , string customerId
            , bool isSuccess
            , string? sessionId = null
            , string? subChannel = null
            , int? deviceId = null
            , string? ipAddress = null
            , string? errorCode = null
            , string? keyword = null
            , string? displayMessage = null
            , string[]? infos = null)
        {
            PayloadMessage payloadMessage = new()
            {
                OperationName = operationName
                ,
                OperationDateTime = operationDateTime
                ,
                Channel = channel
                ,
                CustomerID = customerId
                ,
                Succeeded = isSuccess
                ,
                SessionID = sessionId
                ,
                SubChannel = subChannel
                ,
                DeviceID = deviceId
                ,
                IPAddress = ipAddress
                ,
                ErrorCode = errorCode
                ,
                Keyword = keyword
                ,
                DisplayMessage = displayMessage
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