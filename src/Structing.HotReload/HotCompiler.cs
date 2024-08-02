using McMaster.NETCore.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var mainName = Path.GetFileNameWithoutExtension(MainPluginPath);
            PluginHostLoader = new PluginHostLoader(pluginPath, mainName);
        }
        private IPluginLoadResult? pluginLoadResult;

        public string PluginPath { get; }

        public string ProjectPath { get; }

        public IReadOnlyList<string> Plugins { get; }

        public string MainPluginPath { get; }

        public HotReloader HotReloader { get; }

        public IPluginLoadResult? PluginLoadResult => pluginLoadResult;

        public PluginHostLoader PluginHostLoader { get; }

        public Func<PluginLoader>? PluginLoaderCreator { get; set; }

        public event EventHandler<HotCompilerReloadEventArgs>? Reload;
        public event EventHandler<HotCompilerPluginReloadedEventArgs>? PluginReload;

        public async Task<IPluginLoadResult?> ReloadAsync(PluginHostLoaderReloadOptions options)
        {
            var f = false;
            await HotReloader.CompileAsync();
            PluginHostLoader.PluginLoader?.Reload();
            var loadResult = await PluginHostLoader.ReLoadAsync(options);
            Reload?.Invoke(this, new HotCompilerReloadEventArgs(this, f));
            return loadResult;
        }
    }
}
