using McMaster.NETCore.Plugins;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace Structing.NetCore
{
    public class PluginHostLoader : IDisposable
    {
        class PluginLoadResult : IPluginLoadResult
        {
            public PluginLoadResult(PluginLookupBuildResult buildResult, IServiceProvider serviceProvider)
            {
                BuildResult = buildResult;
                ServiceProvider = serviceProvider;
            }

            public PluginLookupBuildResult BuildResult { get; }

            public IServiceProvider ServiceProvider { get; }

            public void Dispose()
            {
                BuildResult.Modules.StopAsync(ServiceProvider);
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

        public async Task<IPluginLoadResult> ReLoadAsync()
        {
            if (pluginLoader == null)
            {
                pluginLoader = CreateLoader();
            }
            else
            {
                pluginLoader.Reload();
            }
            var lookup = new PluginLookup();
            LookupIniter?.Invoke(lookup);
            var buildResult = lookup.Build(pluginLoader);
            var res = await buildResult.BuildAsync(MainEntitySelector ?? DefaultMainEntitySelector);
            await buildResult.Modules.StartAsync(res);
            return new PluginLoadResult(buildResult, res);
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
