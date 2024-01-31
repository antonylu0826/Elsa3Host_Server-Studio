using Elsa.Email.Activities;
using Elsa.Http;
using Elsa.Workflows.Contracts;
using Elsa.Workflows;
using Elsa.Workflows.Activities;

namespace ElsaServer.Workflows
{
    public class SendEmailWorkflow : WorkflowBase
    {
        protected override void Build(IWorkflowBuilder builder)
        {
            builder.Root = new Sequence
            {
                Activities =
            {
                new HttpEndpoint
                {
                    Path = new("/send-email"),
                    SupportedMethods = new(new[] { HttpMethods.Post }),
                    CanStartWorkflow = true
                },
                new SendEmail
                {
                    From = new("alic@acme.com"),
                    To = new(new[]{ "bob@acme.com" }),
                    Subject = new("Your workflow has been triggered!"),
                    Body = new("Hello!")
                }
            }
            };
        }
    }
}
