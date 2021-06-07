using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Structing.Core;
using Structing.Core.Annotations;
using Microsoft.Extensions.Configuration;
using System;

namespace Structing
{
    public static class ModuleEntryExtensions
    {
        public static void RunRegister(this IModuleRegister moduleEntry, IRegisteContext context)
        {
            if (moduleEntry is null)
            {
                throw new ArgumentNullException(nameof(moduleEntry));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            moduleEntry.ReadyRegister(context);
            moduleEntry.Register(context);
        }
        public static void RunRegister(this IModuleRegister moduleEntry, IServiceCollection services)
        {
            if (moduleEntry is null)
            {
                throw new ArgumentNullException(nameof(moduleEntry));
            }

            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var ctx = new RegisteContext(services);
            moduleEntry.RunRegister(ctx);
        }
        public static async Task RunReadyAsync(this IModuleReady moduleEntry, IReadyContext context)
        {
            if (moduleEntry is null)
            {
                throw new ArgumentNullException(nameof(moduleEntry));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            await moduleEntry.BeforeReadyAsync(context);
            await moduleEntry.ReadyAsync(context);
            await moduleEntry.AfterReadyAsync(context);
        }
        public static async Task<IServiceProvider> RunAsync(this IEnumerable<IModuleEntry> modules,
            IServiceCollection services = null,
            IConfiguration configuration = null,
            IDictionary feature = null)
        {
            if (modules is null)
            {
                throw new ArgumentNullException(nameof(modules));
            }

            if (services == null)
            {
                services = new ServiceCollection();
            }
            var registerCtx = new RegisteContext(services, feature);
            foreach (var item in modules)
            {
                item.RunRegister(registerCtx);
            }
            var provider = services.BuildServiceProvider();
            if (configuration == null)
            {
                configuration = provider.GetService<IConfiguration>();
            }
            var readyContext = new ReadyContext(provider, configuration, feature);
            foreach (var item in modules)
            {
                await item.BeforeReadyAsync(readyContext);
            }
            foreach (var item in modules)
            {
                await item.ReadyAsync(readyContext);
            }
            foreach (var item in modules)
            {
                await item.AfterReadyAsync(readyContext);
            }
            return provider;
        }
    }
}
