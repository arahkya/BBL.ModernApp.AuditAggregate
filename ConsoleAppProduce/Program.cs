// See https://aka.ms/new-console-template for more information
using BBL.ModernApp.AuditAggregate.Client;
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

    var paymentMessage = PayloadMessage.New("TestAudiLog", DateTime.Now, "ConsoleAppProduce", "ConsoleAppProduceClient", true, displayMessage: userEntry);
    Message<string, PayloadMessage> message = new()
    {
        Value = paymentMessage
    };

    producer.Produce("MFHost", message);
}