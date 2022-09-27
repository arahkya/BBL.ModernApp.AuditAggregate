using BBL.ModernApp.AuditAggregate.Contracts;

namespace BBL.ModernApp.AuditAggregate.Client.Dealer;

public interface IDealer
{
    Task SaveAsync(PayloadMessage message);
}