using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Models;

namespace ElsaServer.CustomActivities
{
    public class Greeter : CodeActivity<string>
    {
        public Input<string> HelloName1 { get; set; } = default!;
        public Input<string> HelloName2 { get; set; } = default!;

        protected override void Execute(ActivityExecutionContext context)
        {
            var name = HelloName1.Get(context);
            var message = $"Hello, {name}!";
            context.SetResult(message);
        }
    }
}
