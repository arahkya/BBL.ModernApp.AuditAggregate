using BBL.ModernApp.AuditAggregate.Client.Config;
using BBL.ModernApp.AuditAggregate.Contracts;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

namespace BBL.ModernApp.AuditAggregate.Client.Data;

public class DbDealer
{
    private SqlCommand? _command;
    private readonly DataOption _dataOption;

    private SqlCommand Command
    {
        get
        {
            if (_command == null)
            {
                SqlConnection connection = new(_dataOption.ConnectionString);

                _command = connection.CreateCommand();
                _command.CommandText = "AddAuditLog";
                _command.CommandType = CommandType.StoredProcedure;
            }

            return _command;
        }
    }

    public DbDealer(ClientOption clientOption)
    {
        _dataOption = clientOption.Data;
    }

    public async Task SaveAsync(PayloadMessage message)
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
            int effectedRows = await Command.ExecuteNonQueryAsync();

            if (effectedRows > 0)
            {
                LogWriter.Write($"Processed Message ({message.DisplayMessage})");
            }
        }
        catch (DbException ex)
        {
            throw ex;
        }
        finally
        {
            await Command.Connection.CloseAsync();
        }
    }
}
