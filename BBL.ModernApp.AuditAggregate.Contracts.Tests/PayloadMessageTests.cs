namespace BBL.ModernApp.AuditAggregate.Contracts.Tests
{
    public class PayloadMessageTests
    {
        [Fact]
        public void TestCreateSuccessPayloadMessageMendatoryProperties()
        {
            // Arrange            
            var payloadMessage = PayloadMessage.New(
                "TestAudiLog",
                DateTime.Now,
                "ConsoleAppProduce",
                "ConsoleAppProduceClient",
                true);

            // Action
            payloadMessage.Validate(out bool payloadMessageValid);

            // Assert
            Assert.True(payloadMessageValid);
        }

        [Fact]
        public void TestCreateSuccessPayloadMessageFullProperties()
        {
            // Arrange            
            var payloadMessage = PayloadMessage.New(
                "TestAudiLog",
                DateTime.Now,
                "ConsoleAppProduce",
                "ConsoleAppProduceClient",
                true,
                errorCode: "N/A",
                deviceId: 123456,
                ipAddress: "127.0.0.1",
                subChannel: "Console Application",
                sessionId: Guid.NewGuid().ToString(),
                displayMessage: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi et dolor ornare magna posuere consequat. Fusce vel sagittis diam. Curabitur quam lorem, commodo nec cursus id, dignissim at eros. Sed pellentesque id diam quis scelerisque. Sed non scelerisque sapien. Curabitur vitae est ex. Sed luctus metus ac accumsan tincidunt. Etiam a lectus sit amet augue dignissim vestibulum. Curabitur vitae tempus metus. Fusce vitae venenatis nibh. Curabitur aliquam elit nec tincidunt aliquam.",
                keyword: "MFHost",
                infos: new[] { "info01", "info02", "info03", "info04", "info05", "info06", "info07", "info08", "info09", "info10" });

            // Action
            payloadMessage.Validate(out bool payloadMessageValid);

            // Assert
            Assert.True(payloadMessageValid);
        }

        [Fact]
        public void TestCreateSuccessPayloadMessageSomeInfos()
        {
            // Arrange            
            var payloadMessage = PayloadMessage.New(
                "TestAudiLog",
                DateTime.Now,
                "ConsoleAppProduce",
                "ConsoleAppProduceClient",
                true,
                infos: new[] { "info01", "info02", "info03", "info04" });

            // Action
            payloadMessage.Validate(out bool payloadMessageValid);

            // Assert
            Assert.True(payloadMessageValid);
        }

        [Trait("Required", nameof(PayloadMessage.OperationName))]
        [Trait("Required", nameof(PayloadMessage.CustomerID))]
        [Trait("Required", nameof(PayloadMessage.Channel))]
        [Fact]
        public void TestCreateFailPayloadMessageRequiredFields()
        {
            // Arrange
            static void createPayloadMessage()
            {
                var payloadMessage = PayloadMessage.New(
                    string.Empty,
                    DateTime.Now,
                    string.Empty,
                    string.Empty,
                    true,
                    errorCode: "N/A",
                    deviceId: 123456,
                    ipAddress: "127.0.0.1",
                    subChannel: "Console Application",
                    sessionId: Guid.NewGuid().ToString(),
                    displayMessage: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi et dolor ornare magna posuere consequat. Fusce vel sagittis diam. Curabitur quam lorem, commodo nec cursus id, dignissim at eros. Sed pellentesque id diam quis scelerisque. Sed non scelerisque sapien. Curabitur vitae est ex. Sed luctus metus ac accumsan tincidunt. Etiam a lectus sit amet augue dignissim vestibulum. Curabitur vitae tempus metus. Fusce vitae venenatis nibh. Curabitur aliquam elit nec tincidunt aliquam.",
                    keyword: "MFHost",
                    infos: new[] { "info01", "info02", "info03", "info04", "info05", "info06", "info07", "info08", "info09", "info10" });
            }

            // Action
            PayloadMessageInvalidException exception = Assert.Throws<PayloadMessageInvalidException>(createPayloadMessage);

            // Assert
            Assert.Equal("Payload Creation has Invalid Properties Value.", exception.Message);
            Assert.Contains(exception.ValidationResults, p => p.ErrorMessage == "The Channel field is required.");
            Assert.Contains(exception.ValidationResults, p => p.ErrorMessage == "The CustomerID field is required.");
            Assert.Contains(exception.ValidationResults, p => p.ErrorMessage == "The OperationName field is required.");
        }

        [Trait("Invalid", nameof(PayloadMessage.IPAddress))]
        [Fact]
        public void TestCreateFailPayloadMessageInvalidIPAddress()
        {
            // Arrange
            static void createPayloadMessage()
            {
                var payloadMessage = PayloadMessage.New(
                    "TestAudiLog",
                    DateTime.Now,
                    "ConsoleAppProduce",
                    "ConsoleAppProduceClient",
                    true,
                    errorCode: "N/A",
                    deviceId: 123456,
                    ipAddress: "this is not ip address",
                    subChannel: "Console Application",
                    sessionId: Guid.NewGuid().ToString(),
                    displayMessage: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi et dolor ornare magna posuere consequat. Fusce vel sagittis diam. Curabitur quam lorem, commodo nec cursus id, dignissim at eros. Sed pellentesque id diam quis scelerisque. Sed non scelerisque sapien. Curabitur vitae est ex. Sed luctus metus ac accumsan tincidunt. Etiam a lectus sit amet augue dignissim vestibulum. Curabitur vitae tempus metus. Fusce vitae venenatis nibh. Curabitur aliquam elit nec tincidunt aliquam.",
                    keyword: "MFHost",
                    infos: new[] { "info01", "info02", "info03", "info04", "info05", "info06", "info07", "info08", "info09", "info10" });
            }

            // Action
            PayloadMessageInvalidException exception = Assert.Throws<PayloadMessageInvalidException>(createPayloadMessage);

            // Assert
            Assert.Equal("Payload Creation has Invalid Properties Value.", exception.Message);
            Assert.Contains(exception.ValidationResults, p => p.ErrorMessage == "IP Address not valid URI format.");
        }

        [Trait("Invalid", nameof(PayloadMessage.OperationName))]
        [Fact]
        public void TestCreateFailPayloadMessageInvalidOperationNameLength()
        {
            // Arrange
            static void createPayloadMessage()
            {
                var payloadMessage = PayloadMessage.New(
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi et dolor ornare magna posuere consequat. Fusce vel sagittis diam.",
                    DateTime.Now,
                    "ConsoleAppProduce",
                    "ConsoleAppProduceClient",
                    true,
                    errorCode: "N/A",
                    deviceId: 123456,
                    ipAddress: "localhost",
                    subChannel: "Console Application",
                    sessionId: Guid.NewGuid().ToString(),
                    displayMessage: "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi et dolor ornare magna posuere consequat. Fusce vel sagittis diam. Curabitur quam lorem, commodo nec cursus id, dignissim at eros. Sed pellentesque id diam quis scelerisque. Sed non scelerisque sapien. Curabitur vitae est ex. Sed luctus metus ac accumsan tincidunt. Etiam a lectus sit amet augue dignissim vestibulum. Curabitur vitae tempus metus. Fusce vitae venenatis nibh. Curabitur aliquam elit nec tincidunt aliquam.",
                    keyword: "MFHost",
                    infos: new[] { "info01", "info02", "info03", "info04", "info05", "info06", "info07", "info08", "info09", "info10" });
            }

            // Action
            PayloadMessageInvalidException exception = Assert.Throws<PayloadMessageInvalidException>(createPayloadMessage);

            // Assert
            Assert.Equal("Payload Creation has Invalid Properties Value.", exception.Message);
            Assert.Contains(exception.ValidationResults, p => p.ErrorMessage == "The field OperationName must be a string with a maximum length of 32.");
        }
    }
}