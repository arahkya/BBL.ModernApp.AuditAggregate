using System.ComponentModel;
using System.Net;
using System.Threading.Channels;
using BBL.ModernApp.AuditAggregate.Client.Config;
using BBL.ModernApp.AuditAggregate.Contracts;
using Confluent.Kafka;
using Serilog;

namespace BBL.ModernApp.AuditAggregate.Workers;

public class ListenerWorker
{
    private readonly BackgroundWorker _bgWorker;
    private readonly Channel<PayloadMessage> _dataChannel;
    private IConsumer<Ignore, PayloadMessage> _consumer = null!;

    public bool IsRunning => _bgWorker.IsBusy;

    public ListenerWorker(ClientOption kafkaOption, Channel<PayloadMessage> dataChannel)
    {
        _bgWorker = new BackgroundWorker();
        _bgWorker.WorkerSupportsCancellation = true;
        _bgWorker.DoWork += DoWork;

        InitialWork(kafkaOption.Kafka);
        this._dataChannel = dataChannel;
    }

    public void Stop()
    {
        _consumer.Close();
        _consumer.Dispose();
        _bgWorker.CancelAsync();
        _bgWorker.Dispose();

        Log.Information("Stoped");
    }

    public void Start()
    {
        Log.Information("Running");
        _bgWorker.RunWorkerAsync();
    }

    private void InitialWork(KafkaOption kafkaOption)
    {
        ConsumerConfig config = new()
        {
            BootstrapServers = kafkaOption.Server,
            ClientId = Dns.GetHostName(),
            AutoOffsetReset = AutoOffsetReset.Latest,
            GroupId = kafkaOption.Group,
        };
        ConsumerBuilder<Ignore, PayloadMessage> builder = new(config);
        builder.SetValueDeserializer(new PayloadMessageSerializer());

        _consumer = builder.Build();
        _consumer.Subscribe(kafkaOption.Topic);
    }

    private async void DoWork(object? sender, DoWorkEventArgs e)
    {
        while (!e.Cancel)
        {
            ConsumeResult<Ignore, PayloadMessage> result = _consumer.Consume();
            
            await _dataChannel.Writer.WriteAsync(result.Message.Value);

            Log.Information("Receiving Message...");
        }
    }
}