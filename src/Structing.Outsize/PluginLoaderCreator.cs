using System;
using System.IO;
using System.Linq;
#if !NETSTANDARD
using System.Security.Policy;
#endif
#if NETCOREAPP || NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Structing.Outsize
{
    public static class PluginLoaderCreator
    {
#if NETCOREAPP || NET5_0_OR_GREATER
        public static readonly IPluginLoader Default = DefaultPluginLoader.Instance;

        public static IPluginLoader Create()
        {
            return new PluginLoader(AppDomain.CurrentDomain.BaseDirectory);
        }
#else
        public static readonly IPluginLoader Default = new PluginLoader(AppDomain.CurrentDomain);

        public static IPluginLoader Create()
        {
           return new PluginLoader(Guid.NewGuid().ToString());
        }
#endif
    }
}