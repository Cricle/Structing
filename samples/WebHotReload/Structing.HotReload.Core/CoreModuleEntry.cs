using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Structing.AspNetCore;

namespace Structing.HotReload.WebHost
{
    public class CoreModuleEntry : AutoModuleEntry
    {
        private WebApplicationBuilder? builder;
        private WebApplication? app;

        private string[] args;

        public WebApplicationBuilder? Builder => builder;

        public WebApplication? App => app;

        public CoreModuleEntry()
        {
            this.args =Environment.GetCommandLineArgs();
        }

        public override void ReadyRegister(IRegisteContext context)
        {
            builder = WebApplication.CreateBuilder(args);
            var coll = context.Features[KnowsFeatureKeys.ModulesKey] as ModuleCollection;
            var appPartMgr = new ApplicationPartManager();
            context.SetApplicationPartManager(appPartMgr);
            if (coll != null)
            {
                var cols = new ModuleCollection(coll.Except(new[] { this } ));
                builder.AddModules(cols, context);
            }
            var mvcBuilder=builder.Services.AddMvc();
            foreach (var item in appPartMgr)
            {
                mvcBuilder.AddApplicationPart(item);
            }
            builder.Services.AddSwaggerGen();
            base.ReadyRegister(context);
        }
        public override Task BeforeReadyAsync(IReadyContext context)
        {
            app = builder!.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapDefaultControllerRoute();
            var coll = context.Features[KnowsFeatureKeys.ModulesKey] as ModuleCollection;
            context.SetIEndpointRouteBuilder(app);
            context.SetIApplicationBuilder(app);
            context.SetIConfiguration(app.Configuration);
            context.SetIHost(app);
            if (coll != null)
            {
                var cols = new ModuleCollection(coll.Except(new[] { this }));
                app.UseModule(cols, context);
            }
            context.Features[KnowsFeatureKeys.ServiceProviderKey] = app.Services;
            return base.BeforeReadyAsync(context);
        }
        public override async Task StartAsync(IServiceProvider provider)
        {
            await app!.StartAsync();
            await base.StartAsync(provider);
        }
        public override async Task StopAsync(IServiceProvider provider)
        {
            await base.StopAsync(provider);
            await app!.StopAsync();
        }
    }
}