using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Structing.Core
{
    public static class ModuleEntryExtensions
    {
        public static void RunRegister(this IModuleRegister moduleEntry, IRegisteContext context)
        {
            moduleEntry.ReadyRegister(context);
            moduleEntry.Register(context);
        }
        public static void RunRegister(this IModuleRegister moduleEntry, IServiceCollection services)
        {
            var ctx = new RegisteContext(services);
            moduleEntry.RunRegister(ctx);
        }
        public static async Task RunReadyAsync(this IModuleReady moduleEntry,IReadyContext context)
        {
            await moduleEntry.BeforeReadyAsync(context);
            await moduleEntry.ReadyAsync(context);
            await moduleEntry.AfterReadyAsync(context);
        }
    }
}
