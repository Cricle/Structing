using Microsoft.CodeAnalysis.Emit;
using Structing.NetCore;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Structing.HotReload
{
    public interface IHotReloader : IList<string>
    {
        ICompileResultEmitter Emitter { get; }
        IPluginLookup PluginLookup { get; }
        IProjectCompiler ProjectCompiler { get; }

        Task<IList<EmitResult>> CompileAsync(CancellationToken token = default);
    }
}