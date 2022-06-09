using Structing.Core;
using System.Threading.Tasks;

namespace Structing.Annotations
{
    public interface IModuleInit
    {
        Task InvokeAsync(IReadyContext context);
    }
}
