// See https://aka.ms/new-console-template for more information
using BBL.ModernApp.AuditAggregate.Contracts;
using Confluent.Kafka;
using System.Net;

Console.WriteLine("Hello, World!");

ProducerBuilder<string, PayloadMessage> builder = new(new ProducerConfig()
{
    BootstrapServers = "kafka01:9092",
    ClientId = Dns.GetHostName(),
});

builder.SetValueSerializer(new PayloadMessageSerializer());

using IProducer<string, PayloadMessage> producer = builder.Build();

while (true)
{
    Console.Write("> ");

    string? userEntry = Console.ReadLine();

    if (string.IsNullOrEmpty(userEntry)) continue;    

    var payloadMessage = PayloadMessage.New("TestAudiLog", 
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

    Message<string, PayloadMessage> message = new()
    {
        Value = payloadMessage
    };

    producer.Produce("MFHost", message);
}