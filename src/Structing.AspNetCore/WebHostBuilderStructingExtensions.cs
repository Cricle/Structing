using Microsoft.AspNetCore.Hosting;
using Structing.AspNetCore;
using Structing.Core;

namespace Structing.AspNetCore
{
    public static class WebHostBuilderStructingExtensions
    {
        public static IWebHostBuilder UseStartup(this IWebHostBuilder hostBuilder, params IModuleEntry[] entries)
        {
            return hostBuilder.UseStartup(x => new AspNetCoreStartup(x, new ModuleCollection(entries)));
        }

        public static IWebHostBuilder UseStartup(this IWebHostBuilder hostBuilder, IModuleEntry entry)
        {
            return hostBuilder.UseStartup(x => new AspNetCoreStartup(x, entry));
        }

    }
}
