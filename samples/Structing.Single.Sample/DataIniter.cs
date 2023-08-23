using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using Structing.Core;
using System.Threading.Tasks;

namespace Structing.Single.Sample
{
    [ModuleIniter]
    internal class FeatureRegister : IModuleInit
    {
        public Task InvokeAsync(IReadyContext context)
        {
            context.SetSomeFeature(new SomeFeature { A = 123 });
            return Task.CompletedTask;
        }
    }

    [ModuleIniter]
    internal class DataIniter : IModuleInit
    {
        public Task InvokeAsync(IReadyContext context)
        {
            System.Console.WriteLine(context.GetSomeFeature().A);
            var valSer = context.GetRequiredService<ValueService>();
            valSer.Value = 100;
            return Task.CompletedTask;
        }
    }
}
