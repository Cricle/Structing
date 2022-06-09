using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structing.Single.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = ModuleHelper.RunAssemblyAsync().GetAwaiter().GetResult();

            var ser = provider.GetRequiredService<SayHelloService>();
            ser.SayHello();

            Console.WriteLine(provider.GetRequiredService<ValueService>().Value);
        }
    }
}
