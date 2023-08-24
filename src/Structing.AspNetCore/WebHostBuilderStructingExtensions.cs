using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Structing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Structing.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    public static class WebHostBuilderStructingExtensions
    {
        public const string RegisteContextKey = "Structing.RegisteContext";
        public const string ReadyContextKey = "Structing.ReadyContext";

        internal static T GetOrAdd<T>(IDictionary<object, object> map, string key, T? origin, Func<T> valueFactory)
            where T : class
        {
            if (origin == null)
            {
                if (map.TryGetValue(key, out var obj) && obj is T rgctx)
                {
                    return rgctx;
                }
                var value = valueFactory();
                map.Add(key, value);
                return value;
            }
            return origin;
        }

        public static WebApplicationBuilder AddModules(this WebApplicationBuilder builder, IModuleEntry module, IRegisteContext? context = null)
        {
            builder.Host.ConfigureServices((ctx, services) =>
            {
                var registeContext = GetOrAdd(builder.Host.Properties,
                    RegisteContextKey,
                    context,
                    () => new RegisteContext(services));
                registeContext.SetWebApplicationBuilder(builder);
                module.ReadyRegister(registeContext);
                module.Register(registeContext);
                module.AfterRegister(registeContext);
            });
            return builder;
        }
        public static WebApplication UseModule(this WebApplication builder, IModuleEntry entry, IReadyContext? context = null)
        {
            var ctx = new ReadyContext(builder.Services, context?.Features ?? new Dictionary<object, object>());
            ctx.SetIHost(builder);
            ctx.SetIEndpointRouteBuilder(builder);
            ctx.SetWebApplication(builder);
            UseModule((IApplicationBuilder)builder, entry, ctx);
            return builder;
        }
        public static IApplicationBuilder UseModule(this IApplicationBuilder builder, IModuleEntry entry, IReadyContext? context = null)
        {
            context = context ?? new ReadyContext(builder.ApplicationServices);
            context.SetIApplicationBuilder(builder);
            RunReadyAsync(entry,context).GetAwaiter().GetResult();
            var lifetime = builder.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() => entry.StartAsync(builder.ApplicationServices).GetAwaiter().GetResult());
            lifetime.ApplicationStopping.Register(() => entry.StopAsync(builder.ApplicationServices).GetAwaiter().GetResult());
            return builder;
        }
        private static async Task RunReadyAsync(IModuleEntry entry,IReadyContext context)
        {
            await entry.BeforeReadyAsync(context);
            await entry.ReadyAsync(context);
            await entry.AfterReadyAsync(context);
        }
    }
}
