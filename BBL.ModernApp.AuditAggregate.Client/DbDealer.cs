using System.Data.SqlClient;
using System.Reflection;

namespace BBL.ModernApp.AuditAggregate.Client
{
    public static class DbDealer
    {
        private static SqlCommand? _command;

        private static SqlCommand Command
        {
            get
            {
                if (_command == null)
                {
                    SqlConnection connection = new ("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=bblaudit;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                    SqlDataAdapter adapter = new("select * from dbo.bblaudit", connection);
                    SqlCommandBuilder commandBuilder = new(adapter);
                    _command = commandBuilder.GetInsertCommand(true);
                }

                return _command;
            }
        }



        public static async Task SaveAsync(PayloadMessage message)
        {
            PropertyInfo[] props = message.GetType().GetProperties();

            foreach (SqlParameter param in Command.Parameters)
            {
                string columnName = param.ParameterName.Remove(0, 1);
                param.Value = props.Single(p => p.Name == columnName).GetValue(message) ?? DBNull.Value;
            }

            try
            {
                await Command.Connection.OpenAsync();
                await Command.ExecuteNonQueryAsync();
                

                LogWriter.Write($"Processed Message ({message.DisplayMessage})");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await Command.Connection.CloseAsync();
            }
        }
    }
}