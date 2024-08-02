using McMaster.NETCore.Plugins;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyModel;

namespace Structing.NetCore
{
    public class PluginHostLoader : IDisposable
    {
        class PluginLoadResult : IPluginLoadResult
        {
            public PluginLoadResult(PluginLookupBuildResult buildResult, IServiceProvider? serviceProvider)
            {
                BuildResult = buildResult;
                ServiceProvider = serviceProvider;
            }

            public PluginLookupBuildResult BuildResult { get; }

            public IServiceProvider? ServiceProvider { get; }

            public void Dispose()
            {
                if (ServiceProvider != null)
                {
                    BuildResult.Modules.StopAsync(ServiceProvider);
                }
            }
        }
        public PluginHostLoader(string pluginFolder, string mainPluginName, PluginLoader? pluginLoader = null)
        {
            this.pluginLoader = pluginLoader;
            PluginFolder = pluginFolder;
            MainPluginName = mainPluginName;
            LookupIniter = l => l.AddFolder(pluginFolder);
        }
        private int reloadCount;
        private PluginLoader? pluginLoader;

        public string PluginFolder { get; }

        public string MainPluginName { get; }

        public PluginLoader? PluginLoader => pluginLoader;

        public Func<PluginLookup, PluginLookup>? LookupIniter { get; set; }

        public Func<IModuleEntry, bool>? MainEntitySelector { get; set; }

        public int ReloadCount => Volatile.Read(ref reloadCount);


        private void EnsurePluginLoaderCreated()
        {
            if (pluginLoader == null)
            {
                pluginLoader = CreateLoader();
            }
            else
            {
                pluginLoader.Reload();
            }
        }

        public IReadOnlyDictionary<PluginInfo,DependencyContext> GetDependencyContextMap()
        {
            EnsurePluginLoaderCreated();
            var lookup = new PluginLookup();
            var buildResult = lookup.LoadAssembly(pluginLoader!);
            var res = new Dictionary<PluginInfo, DependencyContext>();
            foreach (var item in buildResult)
            {
                res[item.Key]=DependencyContext.Load(item.Value);
            }
            return res;
        }

        public async Task<IPluginLoadResult> ReLoadAsync(PluginHostLoaderReloadOptions options)
        {
            EnsurePluginLoaderCreated();
            var lookup = new PluginLookup();
            LookupIniter?.Invoke(lookup);
            var buildResult = lookup.Build(pluginLoader!);
            IServiceProvider? provider = null;
            if (options.ReloadMode.HasFlag( PluginReloadMode.Build))
            {
                provider = await buildResult.BuildAsync(MainEntitySelector ?? DefaultMainEntitySelector);
            }
            if (provider != null && options.ReloadMode.HasFlag(PluginReloadMode.Run))
            {
                await buildResult.Modules.StartAsync(provider);
            }
            return new PluginLoadResult(buildResult, provider);
        }

        private bool DefaultMainEntitySelector(IModuleEntry moduleEntry)
        {
            return moduleEntry.GetType().Assembly.GetName().Name == MainPluginName;
        }

        protected virtual PluginLoader CreateLoader()
        {
            var path = Path.Combine(PluginFolder, MainPluginName, MainPluginName + ".dll");
            return CreateDefaultLoader(path);
        }

        private static PluginLoader CreateDefaultLoader(string mainAssemblyPath)
        {
            return new PluginLoader(new PluginConfig(mainAssemblyPath)
            {
                EnableHotReload = true,
                SharedAssemblies =
                {
                    typeof(IModuleEntry).Assembly.GetName()
                }
            });
        }

        public void Dispose()
        {
            pluginLoader?.Dispose();
        }
    }
}
