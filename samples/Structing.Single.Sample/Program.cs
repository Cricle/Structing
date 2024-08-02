using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using System;

[assembly: ModuleEntry]
namespace Structing.Single.Sample;

[EnableService(ServiceLifetime.Singleton)]
internal partial class Program
{
    public int Value;
    static void Main(string[] args)
    {
        //var ser = new ServiceCollection().AddSingleton<Program>().BuildServiceProvider().GetRequiredService<Program>();
        Console.WriteLine(new SampleModuleEntry().Run().GetRequiredService<Program>().Value);
    }
    [ModulePart]
    public static void RegistSome(IRegisteContext context) => Console.WriteLine("Regist some");
    [ModuleIniter]
    internal static void InitSomeAsync(Program program) => Console.WriteLine("Execute InitSomeAsync {0}", program.Value++);
}