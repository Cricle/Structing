using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using System;
using System.Threading.Tasks;

[assembly: ModuleEntry]
namespace Structing.Single.Sample
{
    [EnableService(ServiceLifetime = ServiceLifetime.Singleton)]
    [ModulePart(OnlyCodeGen = true)]
    [ModuleIniter(OnlyCodeGen = true)]
    class Program
    {
        public int Value;
        public void SayHello() => Console.WriteLine("Say hello");

        [ModuleIgnore]
        static void Main(string[] args)
        {
            var provider = new SampleModuleEntry().Run();

            var valueSer = provider.GetRequiredService<Program>();
            valueSer.SayHello();

            Console.WriteLine(valueSer.Value);
        }
        public static void RegistSome(IRegisteContext context) => Console.WriteLine("Execute RegistSome");

        internal static Task InitSomeAsync(IReadyContext context)
        {
            Console.WriteLine("Execute InitSomeAsync");
            return Task.CompletedTask;
        }
    }
}
