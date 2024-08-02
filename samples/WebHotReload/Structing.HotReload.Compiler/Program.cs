using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;
using Structing.HotReload.Exceptions;
using Structing.NetCore;

namespace Structing.HotReload.Compiler
{
    internal class Program
    {
        static HotReloader hotReloader = null!;
        static async Task<int> Main(string[] args)
        {
            //args = new string[]
            //{
            //    "C:\\Users\\huaji\\Workplace\\github\\Structing\\samples\\WebHotReload\\Structing.HotReload.WebHost\\bin\\Debug\\net7.0\\plugins",
            //    "C:\\Users\\huaji\\Workplace\\github\\Structing\\samples\\WebHotReload\\Structing.HotReload.WebHost\\bin\\Debug\\net7.0\\../../../../",
            //    "Structing.HotReload.Core;Structing.HotReload.School"
            //};
            var pluginPath = args[0];
            if (Directory.Exists(pluginPath))
            {
                Directory.Delete(pluginPath, true);
            }
            var projectPath = args[1];
            var plugins = args[2].Split(';');
            MSBuildLocator.RegisterDefaults();
            hotReloader = HotReloader.FromDefault(pluginPath,
                p => Console.WriteLine($"{p.Operation} {p.FilePath} use {p.ElapsedTime.TotalMilliseconds:F4}ms"),
                e =>
                {
                    e.EmitPdb = false;
                });
            var compiler = new HotCompiler(pluginPath, projectPath, plugins, hotReloader);
            var result = await SafeReloadAsync(compiler);
            return result ? 0 : 1;
        }
        private static async Task<bool> SafeReloadAsync(HotCompiler compiler)
        {
            try
            {
                var result = await compiler.ReloadAsync(new PluginHostLoaderReloadOptions { ReloadMode= PluginReloadMode.None});
                if (result != null)
                {
                    var writer = new DependencyContextWriter();
                    foreach (var item in result.BuildResult.AssemblyMaps)
                    {
                        Console.WriteLine("Now generate deps.json");
                        var d =DependencyContext.Load(item.Value);
                        var depCtx = DependencyContext.Load(item.Value);
                        if (depCtx != null)
                        {
                            var depFile = Path.Combine(item.Key.Directory, $"{Path.GetFileNameWithoutExtension(item.Key.FileName)}.deps.json");
                            using (var fs = File.Open(depFile, FileMode.Create))
                            {
                                writer.Write(depCtx, fs);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Fail to load dep ctx");
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
                when (ex is HotCompileException hotEx)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var item in hotEx.EmitResult.Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error))
                {
                    Console.WriteLine(CSharpDiagnosticFormatter.Instance.Format(item));
                }
                Console.ResetColor();
                return false;
            }
        }
    }
}