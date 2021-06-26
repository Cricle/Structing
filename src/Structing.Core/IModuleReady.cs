using System.Threading.Tasks;

namespace Structing.Core
{
    public interface IModuleReady
    {
        Task BeforeReadyAsync(IReadyContext context);
        Task ReadyAsync(IReadyContext context);
        Task AfterReadyAsync(IReadyContext context);
    }
}
