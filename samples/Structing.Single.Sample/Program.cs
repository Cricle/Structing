using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using System;
using System.Threading.Tasks;

[assembly: ModuleEntry]
namespace Structing.Single.Sample
{
    
    [EnableService(ServiceLifetime = ServiceLifetime.Singleton)]
    class Program
    {
        public int Value;
        static void Main(string[] args)
        {
            var provider = new SampleModuleEntry().Run();

            var valueSer = provider.GetRequiredService<Program>();
            valueSer.Value++;

            Console.WriteLine(valueSer.Value);
        }
        [ModulePart]
        public static void RegistSome(IRegisteContext context) => Console.WriteLine("Execute RegistSome");

        [ModuleIniter]
        internal static Task InitSomeAsync(IReadyContext context)
        {
            Console.WriteLine("Execute InitSomeAsync");
            return Task.CompletedTask;
        }
    }
}
