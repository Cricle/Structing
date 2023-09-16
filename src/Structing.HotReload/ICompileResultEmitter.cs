using Microsoft.CodeAnalysis.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace Structing.HotReload
{
    public interface ICompileResultEmitter
    {
        Task<EmitResult> EmitResultAsync(IProjectCompiledResult result, CancellationToken token = default);
    }
}
