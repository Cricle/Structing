using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.NetCore
{
    public readonly record struct PluginLookupBuildResult
    {
        public readonly ModuleCollection Modules;

        public readonly IReadOnlyDictionary<PluginInfo, Assembly> AssemblyMaps;

        public PluginLookupBuildResult(ModuleCollection modules, IReadOnlyDictionary<PluginInfo, Assembly> assemblyMaps)
        {
            Modules = modules;
            AssemblyMaps = assemblyMaps;
        }
        public Task<IServiceProvider> BuildAsync(IServiceCollection? services = null, IDictionary? features = null)
        {
            return BuildAsync(s=>s.BuildServiceProvider(),services, features);
        }
        public async Task<IServiceProvider> BuildAsync(Func<IServiceCollection, IServiceProvider> buildService,IServiceCollection? services=null, IDictionary? features=null)
        {
            services ??= new ServiceCollection();
            features ??= new Dictionary<object, object>();
            var regCtx = new RegisteContext(services, features);
            Modules.RunRegister(regCtx);
            var provider = buildService(services);
            var readyContext = new ReadyContext(provider, features);
            Modules.RunRegister(regCtx);
            await Modules.RunReadyAsync(readyContext);
            return provider;
        }
    }
}
