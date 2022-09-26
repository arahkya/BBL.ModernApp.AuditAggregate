using BBL.ModernApp.AuditAggregate.Contracts;
using System.Data;
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
                    SqlConnection connection = new("Data Source=.,1433;Initial Catalog=bbldevdb;User ID=SA;Password=Password123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                    
                    _command = connection.CreateCommand();
                    _command.CommandText = "AddAuditLog";
                    _command.CommandType = CommandType.StoredProcedure;
                }

                return _command;
            }
        }

        public static async Task SaveAsync(PayloadMessage message)
        {
            PropertyInfo[] props = message.GetType().GetProperties();

            try
            {
                Command.Parameters.Clear();
                foreach (PropertyInfo param in props)
                {
                    SqlParameter commandParam = Command.CreateParameter();
                    commandParam.ParameterName = $"@{param.Name}";
                    commandParam.Value = props.Single(p => p.Name == param.Name).GetValue(message) ?? DBNull.Value;

                    Command.Parameters.Add(commandParam);
                }

                await Command.Connection.OpenAsync();
                await Command.ExecuteNonQueryAsync();


                LogWriter.Write($"Processed Message ({message.DisplayMessage})");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await Command.Connection.CloseAsync();
            }
        }
    }
}