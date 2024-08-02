using Structing.NetCore;
using System.Diagnostics;

namespace Structing.HotReload.Host
{
    class Program
    {
        public static string FolderName => Path.Combine(AppContext.BaseDirectory, "plugins");
        static async Task Main(string[] args)
        {
            var pluginPath = Path.Combine(AppContext.BaseDirectory, FolderName);
            var projectPath = Path.Combine(AppContext.BaseDirectory, "../../../../");
            var loader = new PluginHostLoader(pluginPath, "Structing.HotReload.Core");
            IPluginLoadResult? loadResult = null;
            Console.WriteLine("Compling");
            while (true)
            {
                loadResult?.Dispose();
                var result = Compile(FolderName, projectPath);
                if (result)
                {
                    Console.WriteLine("Compile succeed, now start web host....");
                    loadResult = await loader.ReLoadAsync(new PluginHostLoaderReloadOptions {  ReloadMode= PluginReloadMode.All});
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Compile fail");
                }
            }
        }

        static bool Compile(string pluginPath, string projectPath)
        {
            var ok = true;
            foreach (var item in new string[]
                    {
                        "Structing.HotReload.Core",
                        "Structing.HotReload.School"
                    })
            {
                var csproj = Path.Combine(projectPath, item, item + ".csproj");
                var target = Path.Combine(pluginPath, item);
                var proc = new Process();
                proc.StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments=$"build {csproj} -c Release -f net7.0 -o {target}",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                };
                proc.StartInfo.RedirectStandardOutput = true;
                proc.OutputDataReceived += OnProcOutputDataReceived;
                proc.Start();
                proc.BeginOutputReadLine();
                proc.WaitForExit();
                ok &= proc.ExitCode == 0;
            }
            return ok;
        }

        private static void OnProcOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
