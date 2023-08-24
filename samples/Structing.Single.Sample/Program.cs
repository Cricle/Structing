using Microsoft.Extensions.DependencyInjection;
using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Structing.Single.Sample
{
    [Feature(A.Yellow, Type =typeof(SomeFeature))]
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
    public class SomeFeature
    {
        public int A { get; set; }
    }
    public enum A
    {
        Red,
        Yellow
    }
}
