using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using System.Data;
using System.Data.SqlClient;

namespace Elsa3.Activities.SQLServer
{
    [Activity("Elsa", "SQL Server", "Execute SQL Command and return a DataTable.", Kind = ActivityKind.Task)]
    public class SQLServerGetDataTable : CodeActivity<object>
    {
        [Input(Description = "Data Source connection string.")]
        public Input<string> ConnectionString { get; set; } = default!;

        [Input(Description = "Command Text.")]
        public Input<string> CommandText { get; set; } = default!;

        protected override async void Execute(ActivityExecutionContext context)
        {
            var cancellationToken = context.CancellationToken;
            var result = new DataTable();
            using var conn = new SqlConnection(ConnectionString.Get(context));
            var adapter = new SqlDataAdapter(CommandText.Get(context), conn);
            adapter.Fill(result);
            Result.Set(context, result);
        }
    }
}
