namespace BBL.ModernApp.AuditAggregate.Client.Config;

public class ClientOption
{
    public DataOption Data { get; set; } = null!;
    public FiltersOption Filters { get; set; } = null!;
    public KafkaOption Kafka { get; set; } = null!;
    public int ThreadLimit { get; set; }
}