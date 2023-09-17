using McMaster.NETCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Structing.NetCore;
using System.IO;

namespace Structing.HotReload
{
    public class HotCompiler
    {
        public HotCompiler(string pluginPath, string projectPath, IReadOnlyList<string> plugins, HotReloader hotReloader, string? mainPluginPath = null)
        {
            PluginPath = pluginPath;
            ProjectPath = projectPath;
            Plugins = plugins;
            HotReloader = hotReloader;
            hotReloader.AddCSProjs(projectPath, plugins);
            hotReloader.PluginLookup.AddRange(pluginPath, plugins.Select(x => Path.Combine(x, $"{x}.dll")));
            MainPluginPath = mainPluginPath ?? hotReloader.PluginLookup[0].Path;
        }
        private IServiceProvider? provider;
        private int first;
        private PluginLoader? pluginLoader;
        private PluginLookupBuildResult? buildResult;

        public string PluginPath { get; }

        public string ProjectPath { get; }

        public IReadOnlyList<string> Plugins { get; }

        public string MainPluginPath { get; }

        public HotReloader HotReloader { get; }

        public PluginLoader? PluginLoader => pluginLoader;

        public IServiceProvider? Provider => provider;

        public PluginLookupBuildResult? BuildResult => buildResult;

        public event EventHandler<HotCompilerReloadEventArgs>? Reload;
        public event EventHandler<HotCompilerPluginReloadedEventArgs>? PluginReload;

        public async Task ReloadAsync()
        {
            var f = false;
            await HotReloader.CompileAsync();
            if (Interlocked.CompareExchange(ref first, 1, 0) == 0)
            {
                pluginLoader = new PluginLoader(new PluginConfig(MainPluginPath) 
                {
                    EnableHotReload = true,
                    SharedAssemblies =
                    {
                        typeof(IModuleEntry).Assembly.GetName()
                    }
                });
                pluginLoader.Reloaded += OnPluginReloaded;
                f = true;
            }
            pluginLoader!.Reload();
            Reload?.Invoke(this, new HotCompilerReloadEventArgs(this, f));
        }
        private async void OnPluginReloaded(object sender, PluginReloadedEventArgs eventArgs)
        {
            if (buildResult != null)
            {
                await buildResult.Modules.StopAsync(provider);
            }
            if (provider is IDisposable disposable)
            {
                disposable?.Dispose();
            }
            if (pluginLoader == null)
            {
                throw new InvalidOperationException("Must call ReloadAsync first, then PluginLoader will new instance");
            }
            buildResult = HotReloader.PluginLookup.Build(pluginLoader);
            var mainFileName = Path.GetFileNameWithoutExtension(MainPluginPath);
            provider = await buildResult.BuildAsync(x => x.GetType().Assembly.GetName().Name == mainFileName);
            await buildResult.Modules.StartAsync(provider);
            PluginReload?.Invoke(this, new HotCompilerPluginReloadedEventArgs(this, provider));
        }
    }
}
