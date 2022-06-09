using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Structing.Core;
using Structing.Web;
using System.Collections;

namespace Structing.AspNetCore
{
    public class AspNetCoreStartup : Startup
    {
        public AspNetCoreStartup(WebHostBuilderContext context, IModuleEntry module)
            : base(module)
        {
            Context = context;
        }

        public WebHostBuilderContext Context { get; }

        protected override void Configure(ReadyContext ctx)
        {
            var picker = new AspNetCoreServicePicker(Context.HostingEnvironment, Context.Configuration);
            ctx.Features.Add(FeatureExtensions.ServicePickerKey, picker);
        }

        protected override void Configure(RegisteContext ctx)
        {
            var picker = new AspNetCoreServicePicker(Context.HostingEnvironment, Context.Configuration);
            var builder = AddMvc(ctx);
            if (builder != null)
            {
                ctx.Features.Add(MapExtensions.MvcBuilderKey, builder);
            }
            ctx.Features.Add(MapExtensions.ConfigurationKey, Context.Configuration);
            ctx.Features.Add(FeatureExtensions.ServicePickerKey, picker);
        }
        protected override void OnConfigureServices(IServiceCollection services, RegisteContext ctx)
        {
            var mvc = ctx.Features.Get<IMvcBuilder>(MapExtensions.MvcBuilderKey);
            if (mvc != null)
            {
                var mgr = ctx.Features.GetApplicationPartManager();
                foreach (var item in mgr)
                {
                    mvc.AddApplicationPart(item);
                }
            }
        }

        protected virtual IMvcBuilder AddMvc(RegisteContext ctx)
        {
            return ctx.Services.AddMvc();
        }
    }
}
