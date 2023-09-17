using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.MSBuild;
using Structing.NetCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Structing.HotReload
{
    public class HotReloader : List<string>, IHotReloader
    {
        public HotReloader(IPluginLookup pluginLookup, IProjectCompiler projectCompiler, ICompileResultEmitter emitter)
        {
            PluginLookup = pluginLookup ?? throw new ArgumentNullException(nameof(pluginLookup));
            ProjectCompiler = projectCompiler ?? throw new ArgumentNullException(nameof(projectCompiler));
            Emitter = emitter ?? throw new ArgumentNullException(nameof(emitter));
        }

        public IPluginLookup PluginLookup { get; }

        public IProjectCompiler ProjectCompiler { get; }

        public ICompileResultEmitter Emitter { get; }

        public async Task<IList<EmitResult>> CompileAsync(CancellationToken token = default)
        {
            var results = new List<EmitResult>();
            foreach (var item in this)
            {
                using (var compileResult = await ProjectCompiler.CompileAsync(item, token))
                {
                    var result = await Emitter.EmitResultAsync(compileResult);
                    results.Add(result);
                }
            }
            return results;
        }

        public static HotReloader FromDefault(string basePath, Action<ProjectLoadProgress>? progress = null)
        {
            return new HotReloader(new PluginLookup(), new DefaultProjectCompiler(progress), new PhysicalFileCompileResultEmitter(basePath));
        }
    }
}
