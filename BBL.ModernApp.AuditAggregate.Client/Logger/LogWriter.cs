namespace BBL.ModernApp.AuditAggregate.Client
{
    internal static class LogWriter
    {
        public static void Write(string message) => Console.WriteLine($"{DateTime.Now} - {message}");
    }
}
