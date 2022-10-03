using BBL.ModernApp.AuditAggregate.Client.Config;
using BBL.ModernApp.AuditAggregate.Client.Dealer;
using BBL.ModernApp.AuditAggregate.Contracts;
using BBL.ModernApp.AuditAggregate.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;
using Serilog;

public class Program
{
    public static void Main(string[] agrs)
    {
#if DEBUG 
        string projectPath = Path.GetFullPath("../../../", Environment.CurrentDirectory);
        string appsettingsFilePath = Path.Join(projectPath, "appsettings.develop.json");
#else
        string appsettingsFilePath = Path.Join(Environment.CurrentDirectory, "appsettings.json");
#endif

        if (agrs.FirstOrDefault() != null)
        {
            appsettingsFilePath = Path.Join(Environment.CurrentDirectory, agrs.First());
        }

        ConfigurationBuilder configBuilder = new();
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

        using var log = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        Log.Logger = log;

        Log.Information("Begin");

        ListenerWorker listener = _services.GetRequiredService<ListenerWorker>();
        MessageWorker databaseWorker = _services.GetRequiredService<MessageWorker>();
        AppDomain.CurrentDomain.ProcessExit += (s, e) =>
        {
            listener.Stop();
            databaseWorker.Stop();
            Log.Information("End");
        };

        listener.Start();
        databaseWorker.Start();
        Console.Read();
    }
}