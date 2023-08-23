using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Structing.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structing.AspNetCore
{
    public class Startup
    {
        public IModuleEntry Module { get; }

        public Startup(IModuleEntry module)
        {
            Module = module;
        }
        private readonly ApplicationPartManager partMgr = new ApplicationPartManager();
        public void ConfigureServices(IServiceCollection services)
        {
            var ctx = new RegisteContext(services);
            services.AddSingleton(partMgr);
            ctx.Features.Add(FeatureExtensions.ApplicationPartManagerKey, partMgr);
            Configure(ctx);
            Module.ReadyRegister(ctx);
            Module.Register(ctx);
            OnConfigureServices(services, ctx);
        }

        protected virtual void Configure(RegisteContext ctx)
        {

        }
        protected virtual void OnConfigureServices(IServiceCollection services, RegisteContext ctx)
        {

        }
        public void Configure(IApplicationBuilder app)
        {
            var map = new Dictionary<string, object>
            {
                [FeatureExtensions.ApplicationBuilderKey] = app
            };
            var ctx = new ReadyContext(app.ApplicationServices, map);
            ctx.Features.Add(FeatureExtensions.ApplicationPartManagerKey, partMgr);
            Configure(ctx);
            ReadyAsync(ctx).GetAwaiter().GetResult();
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() => Module.StartAsync(app.ApplicationServices).GetAwaiter().GetResult());
            lifetime.ApplicationStopping.Register(() => Module.StopAsync(app.ApplicationServices).GetAwaiter().GetResult());
            OnConfigure(app, ctx);
        }
        protected virtual void Configure(ReadyContext ctx)
        {

        }
        protected virtual void OnConfigure(IApplicationBuilder app, ReadyContext ctx)
        {

        }
        private async Task ReadyAsync(IReadyContext ctx)
        {
            await Module.BeforeReadyAsync(ctx);
            await Module.ReadyAsync(ctx);
            await Module.AfterReadyAsync(ctx);
        }

    }
}
