using BBL.ModernApp.AuditAggregate.Client.Config;
using BBL.ModernApp.AuditAggregate.Client.Data;
using BBL.ModernApp.AuditAggregate.Contracts;
using BBL.ModernApp.AuditAggregate.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

public class Program
{
    public static void Main(string[] agrs)
    {
        ConfigurationBuilder configBuilder = new();
        string appsettingsFilePath = Path.Join(Environment.CurrentDirectory, "Config", "appsettings.json");
        configBuilder.AddJsonFile(appsettingsFilePath, false, true);

        ClientOption clientOption = new();
        IConfigurationRoot configure = configBuilder.Build();
        configure.Bind(clientOption);

        ServiceCollection services = new ServiceCollection();
        services.AddSingleton<Channel<PayloadMessage>>((ServiceProvider) => Channel.CreateUnbounded<PayloadMessage>());
        services.AddScoped<ClientOption>((serviceProvider) => clientOption);
        services.AddScoped<DbDealer>();
        services.AddSingleton<ListenerWorker>();
        services.AddSingleton<MessageWorker>();

        IServiceProvider _services = services.BuildServiceProvider();

        Console.WriteLine("Begin");

        ListenerWorker listener = _services.GetRequiredService<ListenerWorker>();
        MessageWorker databaseWorker = _services.GetRequiredService<MessageWorker>();
        AppDomain.CurrentDomain.ProcessExit += (s, e) =>
        {
            listener.Stop();
            databaseWorker.Stop();
            Console.WriteLine("End");
        };

        listener.Start();
        databaseWorker.Start();
        Console.Read();
    }
}