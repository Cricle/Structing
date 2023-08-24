using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Structing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            moduleEntry.AfterRegister(context);
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
            moduleEntry.RunRegister(new RegisteContext(services));
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
        public static Task<IModuleEntryRunResult> RunAsync(this IModuleEntry modules,
            IServiceCollection services = null,
            IConfiguration configuration = null,
            IDictionary feature = null)
        {
            if (modules is null)
            {
                throw new ArgumentNullException(nameof(modules));
            }

            return RunAsync(new IModuleEntry[] { modules }, services, configuration, feature);
        }
        public static IModuleEntryRunResult Run(this IModuleEntry modules,
            IServiceCollection services = null,
            IConfiguration configuration = null,
            IDictionary feature = null)
        {
            return RunAsync(modules, services, configuration, feature).GetAwaiter().GetResult();
        }
        class ModuleEntryRunResult : IModuleEntryRunResult, IServiceProvider
        {
            public IEnumerable<IModuleEntry> ModuleEntries { get; set; }

            public IServiceCollection Services { get; set; }

            public IConfiguration Configuration { get; set; }

            public IDictionary Feature { get; set; }

            public IServiceProvider ServiceProvider { get; set; }

            public object GetService(Type serviceType)
            {
                return ServiceProvider.GetService(serviceType);
            }
        }
        public static async Task<IModuleEntryRunResult> RunAsync(this IEnumerable<IModuleEntry> modules,
            IServiceCollection services = null,
            IConfiguration configuration = null,
            IDictionary feature = null)
        {
            if (modules is null)
            {
                throw new ArgumentNullException(nameof(modules));
            }
            if (services==null)
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
            var res = new ModuleEntryRunResult
            {
                ServiceProvider = provider,
                Configuration = configuration,
                Feature = feature,
                ModuleEntries = modules,
                Services = services
            };
            return res;
        }
    }
}
