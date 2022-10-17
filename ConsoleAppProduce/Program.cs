// See https://aka.ms/new-console-template for more information
using BBL.ModernApp.AuditAggregate.Contracts;
using BBL.ModernApp.AuditAggregate.Contracts.Producer;

Console.WriteLine("Begin Produce.");

while (true)
{
    Console.Write("> ");

    string? userEntry = Console.ReadLine();

    if (string.IsNullOrEmpty(userEntry)) continue;    

    var payloadMessage = PayloadMessage.New("TestAuditLog", 
        DateTime.Now, 
        "ConsoleAppProduce", 
        "ConsoleAppProduceClient", 
        true,
        errorCode: "N/A",
        deviceId: 123456,
        ipAddress: "127.0.0.1",
        subChannel: "Console Application",
        sessionId: Guid.NewGuid().ToString(),
        displayMessage: userEntry,
        keyword: "MFHost",
        infos: new[] { "info01", "info02", "info03", "info04", "info05", "info06", "info07", "info08", "info09", "info10" });

    MessageProducer producer = new("kafka01:9092", "MFHost");
    producer.Send(payloadMessage);
}