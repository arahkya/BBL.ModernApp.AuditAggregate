using BBL.ModernApp.AuditAggregate.Client;
using BBL.ModernApp.AuditAggregate.Contracts;
using Confluent.Kafka;
using System.Net;

Queue<PayloadMessage> queue = new();

LogWriter.Write("Begin Listen to MFHost");

ConsumerConfig config = new()
{
    BootstrapServers = "kafka01:9092",
    ClientId = Dns.GetHostName(),
    AutoOffsetReset = AutoOffsetReset.Latest,
    GroupId = "audit_client_01",
};

ConsumerBuilder<Ignore, PayloadMessage> builder = new(config);
builder.SetValueDeserializer(new PayloadMessageSerializer());

using IConsumer<Ignore, PayloadMessage> consumer = builder.Build();

consumer.Subscribe("MFHost");

Task consumeTask = Task.Run(() =>
{
    while (true)
    {
        ConsumeResult<Ignore, PayloadMessage>? result = null;

        try
        {
            LogWriter.Write("Listen");

            result = consumer.Consume();

            LogWriter.Write("Got Message");

            queue.Enqueue(result.Message.Value);
        }
        catch
        {
            LogWriter.Write($"Error on Message ({result?.Message.Value.OperationName ?? "N/A"})");
        }
    }
});
Task workTask = Task.Run(async () =>
{
    while (true)
    {
        if (!queue.Any())
        {
            Task.Delay(1000).Wait();
            continue;
        }

        PayloadMessage message = queue.Dequeue();

        try
        {
            await DbDealer.SaveAsync(message);
        }
        catch
        {
            queue.Enqueue(message);
        }
    }
});

await Task.WhenAll(new[] { consumeTask, workTask });

LogWriter.Write("Exit");