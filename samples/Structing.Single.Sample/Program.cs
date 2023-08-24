using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using System;

[assembly: ModuleEntry]

namespace Structing.Single.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new SampleModuleEntry().RunAsync().GetAwaiter().GetResult();

            var ser = provider.GetRequiredService<SayHelloService>();
            ser.SayHello();

            Console.WriteLine(provider.GetRequiredService<ValueService>().Value);
        }
        [ModulePart]
        public static void RegistSome(IRegisteContext context)
        {
            Console.WriteLine("Execute RegistSome");
        }
    }
}
