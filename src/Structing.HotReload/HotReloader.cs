using Microsoft.CodeAnalysis.Emit;
using Structing.NetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Structing.HotReload
{
    public class HotReloader : List<string>
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
    }
}
