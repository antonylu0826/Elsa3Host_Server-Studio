using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using System.Data.SqlClient;

namespace Elsa3.Activities.SQLServer
{
    [Activity("Elsa", "SQL Server", "Execute SQL Command with Scalar.", Kind = ActivityKind.Task)]
    public class SQLServerExecuteScalar : CodeActivity<object>
    {
        [Input(Description = "Data Source connection string.")]
        public Input<string> ConnectionString { get; set; } = default!;

        [Input(Description = "Command Text.")]
        public Input<string> CommandText { get; set; } = default!;

        protected override async void Execute(ActivityExecutionContext context)
        {
            var cancellationToken = context.CancellationToken;
            using var conn = new SqlConnection(ConnectionString.Get(context));
            conn.Open();
            var commandText = CommandText.Get(context);
            var cmd = new SqlCommand(commandText, conn);
            var result = await cmd.ExecuteScalarAsync(cancellationToken);
            conn.Close();
            Result.Set(context, result);
        }
    }
}
