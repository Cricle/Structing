using McMaster.NETCore.Plugins;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.Extensions.DependencyInjection;
using Structing.HotReload.Contract;
using Structing.NetCore;

namespace Structing.HotReload.Host
{
    class Program
    {
        public const string FolderName = "plugins";
        private static string[] plugins = new string[]
        {
            "Structing.HotReload.PluginA",
            "Structing.HotReload.PluginB"
        };
        static HotReloader hotReloader;
        static PluginLoader pluginLoader;
        static async Task Main(string[] args)
        {
            List<string> assemblyNames = null;
            var pluginPath = Path.Combine(AppContext.BaseDirectory, FolderName);
            var projectPath = Path.Combine(AppContext.BaseDirectory, "../", "../", "../", "../"); ;
            MSBuildLocator.RegisterDefaults();
            hotReloader = new HotReloader(new PluginLookup(), new DefaultProjectCompiler(p =>
            {
                Console.WriteLine($"{p.Operation} {p.FilePath} use {p.ElapsedTime.TotalMilliseconds:F4}ms");
            }), new PhysicalFileCompileResultEmitter(pluginPath));
            hotReloader.AddRange(plugins.Select(x => Path.Combine(projectPath, x, $"{x}.csproj")));
            foreach (var item in plugins)
            {
                hotReloader.PluginLookup.Add(Path.Combine(pluginPath, item, $"{item}.dll"));
            }
            await hotReloader.CompileAsync();
            pluginLoader = new PluginLoader(new PluginConfig(Path.Combine(pluginPath, plugins[0],plugins[0] + ".dll")) { EnableHotReload = true });;
            pluginLoader.Reloaded += OnPluginReloaded;
            await hotReloader.CompileAsync();
            OnPluginReloaded(null, new PluginReloadedEventArgs(pluginLoader));
            while (true)
            {
                Console.WriteLine("Any key to compile plugin");
                Console.ReadKey();
                if (Directory.Exists(FolderName))
                {
                    Directory.Delete(FolderName, true);
                }
                await hotReloader.CompileAsync();
                pluginLoader.Reload();
            }
        }
        private static IServiceProvider? provider;
        private static async void OnPluginReloaded(object sender, PluginReloadedEventArgs eventArgs)
        {
            if (provider is IDisposable disposable)
            {
                disposable?.Dispose();
            }
            var result = hotReloader.PluginLookup.Build(pluginLoader);
            provider = await result.BuildAsync();
            var sayers = provider.GetServices<ISayer>();
            foreach (var item in sayers)
            {
                item.Say();
            }
        }
    }
}
