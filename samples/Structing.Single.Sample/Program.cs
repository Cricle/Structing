using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using Structing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Structing.Single.Sample
{
    [Feature(A.Yellow, Type =typeof(SomeFeature))]
    class Program
    {
        static void Main(string[] args)
        {
            new W().Run();
            var provider = ModuleHelper.RunAssemblyAsync().GetAwaiter().GetResult();

            var ser = provider.GetRequiredService<SayHelloService>();
            ser.SayHello();

            Console.WriteLine(provider.GetRequiredService<ValueService>().Value);
        }
        [ModulePart]
        public static void RegistSome(IRegisteContext context)
        {

        }
    }
    [ModuleEntry]
    public partial class ModuleEntry:AutoModuleEntry
    {
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
    partial class W
    {
        public void Run()
        {
            Say();
        }

        partial void Say();
    }
}
