namespace BBL.ModernApp.AuditAggregate.Client.Config;

public class KafkaOption
{
    public string Server { get; set; } = null!;
    public string Group { get; set; } = null!;
    public string Topic { get; set; } = null!;
}
