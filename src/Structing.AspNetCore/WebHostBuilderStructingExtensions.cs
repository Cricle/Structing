using Structing.AspNetCore;
using Structing.Core;

namespace Microsoft.AspNetCore.Hosting
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
