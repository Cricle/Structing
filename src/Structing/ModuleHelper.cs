using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing
{
    public static class ModuleHelper
    {
        public static Task<IServiceProvider> RunAssemblyAsync(Assembly assembly,
            IServiceCollection services = null,
            IConfiguration configuration = null,
            IDictionary feature = null)
        {
            var entity = new ThisModuleEntry(assembly);
            return entity.RunAsync(services, configuration, feature);
        }
#if NETSTANDARD2_0
        public static Task<IServiceProvider> RunAssemblyAsync(
            IServiceCollection services = null,
            IConfiguration configuration = null,
            IDictionary feature = null)
        {
            var entity = new ThisModuleEntry(Assembly.GetCallingAssembly());
            return entity.RunAsync(services, configuration, feature);
        }
#endif
    }
}
