using Structing.Annotations;
using Structing.Core;
using System.Threading.Tasks;

namespace NullModule
{
    [ModuleIniter]
    public class NullIniter : IModuleInit
    {
        public static bool IsInvokeAsync { get; set; }
        public Task InvokeAsync(IReadyContext context)
        {
            IsInvokeAsync = true;
            return Task.FromResult(0);
        }
    }
}
