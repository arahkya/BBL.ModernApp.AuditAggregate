namespace BBL.ModernApp.AuditAggregate.Client.Config;

public class ClientOption
{
    public DataOption Data { get; set; } = null!;
    public KafkaOption Kafka { get; set; } = null!;
}