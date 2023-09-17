using Microsoft.Build.Locator;

namespace Structing.HotReload.Host
{
    class Program
    {
        public const string FolderName = "plugins";
        private static string[] plugins = new string[]
        {
            "Structing.HotReload.Core",
            "Structing.HotReload.School"
        };
        static HotReloader hotReloader = null!;
        static async Task Main(string[] args)
        {
            var pluginPath = Path.Combine(AppContext.BaseDirectory, FolderName);
            var projectPath = Path.Combine(AppContext.BaseDirectory, "../../../../");
            MSBuildLocator.RegisterDefaults();
            hotReloader = HotReloader.FromDefault(pluginPath,
                p => Console.WriteLine($"{p.Operation} {p.FilePath} use {p.ElapsedTime.TotalMilliseconds:F4}ms"));
            var compiler = new HotCompiler(pluginPath, projectPath, plugins, hotReloader);
            compiler.PluginReload += OnCompilerPluginReload;
            await compiler.ReloadAsync();
            while (true)
            {
                Console.WriteLine("Any key to compile plugin");
                Console.ReadKey();
                if (Directory.Exists(FolderName))
                {
                    Directory.Delete(FolderName, true);
                }

                await compiler.ReloadAsync();
            }
        }

        private static void OnCompilerPluginReload(object? sender, HotCompilerPluginReloadedEventArgs e)
        {
        }
    }
}
