using System.ComponentModel;
using System.Threading.Channels;
using BBL.ModernApp.AuditAggregate.Client.Config;
using BBL.ModernApp.AuditAggregate.Client.Dealer;
using BBL.ModernApp.AuditAggregate.Contracts;
using Serilog;

namespace BBL.ModernApp.AuditAggregate.Workers;

public class MessageWorker
{
    private readonly BackgroundWorker _bgWorker;
    private readonly ClientOption _clientOption;
    private readonly Channel<PayloadMessage> _dataChannel;

    public MessageWorker(ClientOption clientOption, Channel<PayloadMessage> dataChannel)
    {
        _bgWorker = new BackgroundWorker();
        _bgWorker.WorkerSupportsCancellation = true;
        _bgWorker.DoWork += DoWork;
        _clientOption = clientOption;
        _dataChannel = dataChannel;
    }

    private async void DoWork(object? sender, DoWorkEventArgs e)
    {
        ParameterizedThreadStart startThreadDbDealer = new ParameterizedThreadStart(async (param) =>
        {
            if (param is PayloadMessage payloadMessage)
            {
                DbDealer dbDealer = new DbDealer(_clientOption);
                await dbDealer.SaveAsync(payloadMessage);
            }
        });

        while (await _dataChannel.Reader.WaitToReadAsync())
        {
            while (_dataChannel.Reader.TryRead(out PayloadMessage? payloadMessage))
            {
                Log.Information($"Processing Message ({payloadMessage?.DisplayMessage ?? "N/A"})");

                Thread threadProcess = new Thread(startThreadDbDealer);
                threadProcess.Start((object?)payloadMessage);
            }
        }
    }

    public void Stop()
    {
        _bgWorker.CancelAsync();
        _bgWorker.Dispose();
    }

    public void Start()
    {
        _bgWorker.RunWorkerAsync();
    }
}