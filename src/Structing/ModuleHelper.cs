using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Structing.Core;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing
{
    public static class ModuleHelper
    {
        public static Task<IModuleEntryRunResult> RunAssemblyAsync(Assembly assembly,
            IServiceCollection services = null,
            IConfiguration configuration = null,
            IDictionary feature = null)
        {
            var entity = new ThisModuleEntry(assembly);
            return entity.RunAsync(services, configuration, feature);
        }
        public static Task<IModuleEntryRunResult> RunAssemblyAsync(
                    IServiceCollection services = null,
                    IConfiguration configuration = null,
                    IDictionary feature = null)
        {
            var entity = new ThisModuleEntry(Assembly.GetCallingAssembly());
            return entity.RunAsync(services, configuration, feature);
        }
    }
}
