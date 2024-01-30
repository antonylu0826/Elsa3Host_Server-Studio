using Elsa.Features.Services;
using Elsa3.Activities.SQLServer.Features;

namespace Elsa.Extensions
{
    public static class ModuleExtensions
    {
        public static IModule UseSQLServer(this IModule configuration, Action<SQLServerFeatures>? configure = default)
        {
            configuration.Configure(configure);
            return configuration;
        }
    }
}
