using System;
using System.Collections.Generic;
using System.Reflection;
#if NETCOREAPP || NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Structing.Outsize
{
    public static class AssemblyModuleContext
    {
        public static IEnumerable<Type> GetModuleType(this IEnumerable<Assembly> assemblies)
        {
            foreach (var item in assemblies)
            {
                foreach (var type in GetModuleType(item))
                {
                    yield return type;
                }
            }
        }
        public static IEnumerable<Type> GetModuleType(this Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            var len = types.Length;
            for (int i = 0; i < len; i++)
            {
                var type = types[i];
                if (type.GetInterface(PluginLoaderUseExtensions.IModuleEntryTypeName) != null)
                {
                    yield return type;
                }
            }
        }
    }
}
