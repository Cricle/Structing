using System.Threading;
using System.Threading.Tasks;

namespace Structing.HotReload
{
    public interface IProjectCompiler
    {
        Task<IProjectCompiledResult> CompileAsync(string csprojPath, CancellationToken token = default);
    }
}
