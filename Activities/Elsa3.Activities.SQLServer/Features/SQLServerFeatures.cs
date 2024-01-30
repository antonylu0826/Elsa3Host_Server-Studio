using Elsa.Extensions;
using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using System.Data;

namespace Elsa3.Activities.SQLServer.Features
{
    public class SQLServerFeatures : FeatureBase
    {
        public SQLServerFeatures(IModule module) : base(module) { }

        public override void Configure()
        {
            Module.UseWorkflowManagement(management =>
            {
                management.AddVariableTypes(new[]
                {
                    typeof(DataTable),
                }, "SqlClient");

                management.AddActivitiesFrom<SQLServerFeatures>();
            });

        }
    }
}
