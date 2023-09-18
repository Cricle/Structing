using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Structing.HotReload.Exceptions;

namespace Structing.HotReload.Compiler
{
    internal class Program
    {
        static HotReloader hotReloader = null!;
        static async Task<int> Main(string[] args)
        {
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
                await compiler.ReloadAsync();
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