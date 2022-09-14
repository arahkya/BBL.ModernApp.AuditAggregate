using Confluent.Kafka;
using System.Data.SqlClient;
using System.Net;


Action<string> writeLog = (message) =>
{
    Console.WriteLine($"{DateTime.Now.ToString()} - {message}");
};

writeLog("Begin Listen to MFHost");

ConsumerConfig config = new()
{
    BootstrapServers = "kafka01:9092",
    ClientId = Dns.GetHostName(),
    AutoOffsetReset = AutoOffsetReset.Latest,
    GroupId = "audit_client_01"
};

ConsumerBuilder<Ignore, string> consumerBuilder = new(config);

using IConsumer<Ignore, string> consumer = consumerBuilder.Build();
consumer.Subscribe("MFHost");


await Task.Run(() =>
{
    while (true)
    {
        ConsumeResult<Ignore, string>? result = null;

        try
        {
            writeLog("Listen");

            result = consumer.Consume();

            writeLog($"Got Message. ({result.Message.Value})");

            Task.Run(async () =>
            {                
                SqlConnection sqlConnection = new ("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=bblaudit;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                SqlCommand command = sqlConnection.CreateCommand();
                command.CommandText = "insert into dbo.bblaudit(CreatedOn,Id,Source,Message) values(@createDate, @id, @source, @message)";

                SqlParameter createDateParam = command.CreateParameter();
                SqlParameter idParam = command.CreateParameter();
                SqlParameter sourceParam = command.CreateParameter();
                SqlParameter messageParam = command.CreateParameter();

                createDateParam.ParameterName = "@createDate";
                idParam.ParameterName = "@id";
                sourceParam.ParameterName = "@source";
                messageParam.ParameterName = "@message";

                createDateParam.Value = DateTime.Now;
                idParam.Value = Guid.NewGuid().ToString();
                sourceParam.Value = "MFHost";
                messageParam.Value = result.Message.Value!;

                command.Parameters.Add(createDateParam);
                command.Parameters.Add(idParam);
                command.Parameters.Add(sourceParam);
                command.Parameters.Add(messageParam);

                try
                {
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    await command.Connection.CloseAsync();

                    writeLog($"Processed Message ({result.Message.Value})");
                }
                catch (Exception)
                {
                    throw;
                }
            });            
        }
        catch
        {
            writeLog($"Error on Message ({result?.Message.Value ?? "N/A"})");
        }
    }    
});

writeLog("Exit");