using Structing.Core;

namespace Structing.AspNetCore.Sample
{
    public class Program
    {
        internal static readonly ModuleCollection modules = new ModuleCollection
        {
            new MainModule(),
            new Module1()
        };

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddModules(modules);
            var app = builder.Build();
            app.UseModule(modules);
            app.Run();
        }
    }
    public class Module1 : AutoModuleEntry
    {
        public override void Register(IRegisteContext context)
        {
            base.Register(context);
            Console.WriteLine(context.GetFeatureCount().A);
        }
    }
    public class MainModule : AutoModuleEntry
    {
        public override void Register(IRegisteContext context)
        {
            base.Register(context);
            var fc = new FeatureCount();
            context.Services.AddSingleton(fc);
            context.SetFeatureCount(fc);
        }
        public override Task ReadyAsync(IReadyContext context)
        {
            context.GetWebApplication()
                .MapGet("/", ctx => ctx.Response.WriteAsync(ctx.RequestServices.GetRequiredService<FeatureCount>().A++.ToString()));
            return base.ReadyAsync(context);
        }
    }
}