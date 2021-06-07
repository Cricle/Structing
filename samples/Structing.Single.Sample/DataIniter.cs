using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using Structing.Core;
using System.Threading.Tasks;

namespace Structing.Single.Sample
{
    [ModuleIniter]
    internal class DataIniter : IModuleInit
    {
        public Task InvokeAsync(IReadyContext context)
        {
            var valSer = context.GetRequiredService<ValueService>();
            valSer.Value = 100;
            return Task.CompletedTask;
        }
    }
}
