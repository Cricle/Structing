using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.NetCore
{
    public class PluginLookupBuildResult
    {
        public readonly ModuleCollection Modules;

        public readonly IReadOnlyDictionary<PluginInfo, Assembly> AssemblyMaps;

        public PluginLookupBuildResult(ModuleCollection modules, IReadOnlyDictionary<PluginInfo, Assembly> assemblyMaps)
        {
            Modules = modules;
            AssemblyMaps = assemblyMaps;
        }
        public Task<IServiceProvider> BuildAsync(Func<IModuleEntry, bool> mainEntitySelector,IServiceCollection? services = null, IDictionary? features = null)
        {
            return BuildAsync(s=>s.BuildServiceProvider(), mainEntitySelector, services, features);
        }
        public async Task<IServiceProvider> BuildAsync(Func<IServiceCollection, IServiceProvider> buildService, Func<IModuleEntry, bool> mainEntitySelector, IServiceCollection? services=null, IDictionary? features=null)
        {
            services ??= new ServiceCollection();
            features ??= new Dictionary<object, object>();
            services.AddSingleton(Modules);
            services.AddSingleton(AssemblyMaps);
            services.AddSingleton(this);
            features.Add(KnowsFeatureKeys.ModulesKey, Modules);
            features.Add(KnowsFeatureKeys.AssemblyMapsKey, AssemblyMaps);
            var regCtx = new RegisteContext(services, features);
            var mainEntity = Modules.First(mainEntitySelector);
            var provider = buildService(services);
            var readyContext = new ReadyContext(provider, features);
            mainEntity.RunRegister(regCtx);
            await mainEntity.RunReadyAsync(readyContext);
            return provider;
        }
    }
}
