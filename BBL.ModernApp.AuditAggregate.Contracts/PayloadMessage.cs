using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace BBL.ModernApp.AuditAggregate.Contracts
{
    public sealed class PayloadMessage
    {        
        [MaxLength(50)]
        public string? SessionID { get; internal set; }

        public DateTime OperationDateTime { get; internal set; } = DateTime.MinValue;

        [MaxLength(32)]
        public string OperationName { get; internal set; } = null!;

        [MaxLength(32)]
        public string CustomerID { get; internal set; } = null!;

        [MaxLength(50)]
        public string Channel { get; internal set; } = null!;

        [MaxLength(50)]
        public string? SubChannel { get; internal set; }
                
        public Int32? DeviceID { get; internal set; }

        [MaxLength(32)]
        public string? IPAddress { get; internal set; }

        public bool Succeeded { get; internal set; }

        [MaxLength(255)]
        public string? ErrorCode { get; internal set; }

        [MaxLength(100)]
        public string? Info1 { get; internal set; }

        [MaxLength(100)]
        public string? Info2 { get; internal set; }

        [MaxLength(100)]
        public string? Info3 { get; internal set; }

        [MaxLength(100)]
        public string? Info4 { get; internal set; }

        [MaxLength(100)]
        public string? Info5 { get; internal set; }

        [MaxLength(100)]
        public string? Info6 { get; internal set; }

        [MaxLength(255)]
        public string? Info7 { get; internal set; }

        [MaxLength(255)]
        public string? Info8 { get; internal set; }

        [MaxLength(255)]
        public string? Info9 { get; internal set; }

        [MaxLength(255)]
        public string? Info10 { get; internal set; }

        [MaxLength(255)]
        public string? Keyword { get; internal set; }

        [MaxLength(1000)]
        public string? DisplayMessage { get; internal set; } = null!;

        [JsonConstructor]
        internal PayloadMessage() { }

        /// <summary>
        /// This is only way to create Payload Message
        /// </summary>
        /// <param name="operationName"></param>
        /// <param name="operationDateTime"></param>
        /// <param name="channel"></param>
        /// <param name="customerId"></param>
        /// <param name="isSuccess"></param>
        /// <param name="sessionId"></param>
        /// <param name="subChannel"></param>
        /// <param name="deviceId"></param>
        /// <param name="ipAddress"></param>
        /// <param name="errorCode"></param>
        /// <param name="keyword"></param>
        /// <param name="displayMessage"></param>
        /// <param name="infos"></param>
        /// <returns></returns>
        /// <exception cref="PayloadMessageInvalidException"></exception>
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

            List<ValidationResult> validationResults = payloadMessage.Validate(out bool isValid);

            if (!isValid)
            {
                throw new PayloadMessageInvalidException(validationResults);
            }

            return payloadMessage;
        }

        public List<ValidationResult> Validate(out bool isValid)
        {
            ValidationContext validationContext = new(this);
            List<ValidationResult> validationResult = new();

            if(OperationDateTime == DateTime.MinValue)
            {
                validationResult.Add(new ValidationResult("Cannot equal to DateTime.MinValue", new[] { nameof(OperationDateTime) }));
            }

            if(IPAddress != null)
            {
                try
                {
                    Uri _ = new (IPAddress);
                }
                catch
                {
                    validationResult.Add(new ValidationResult("IP Address not valid URI format.", new[] { nameof(IPAddress) }));
                }
            }

            isValid = Validator.TryValidateObject(this, validationContext, validationResult);

            return validationResult;
        }
    }
}