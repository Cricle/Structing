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
                    loadResult = await loader.ReLoadAsync();
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
            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(AppContext.BaseDirectory, "compiler", "Structing.HotReload.Compiler.exe"),
                ArgumentList =
                {
                    pluginPath,
                    projectPath,
                    string.Join(";",new string[]
                    {
                        "Structing.HotReload.Core",
                        "C:\\Users\\huaji\\Workplace\\github\\Structing\\samples\\WebHotReload\\Structing.HotReload.School\\Structing.HotReload.School.csproj"
                    })
                },
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
            return proc.ExitCode == 0;
        }

        private static void OnProcOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
