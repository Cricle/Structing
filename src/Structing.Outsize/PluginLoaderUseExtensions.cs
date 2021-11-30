using Structing.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Structing.Outsize
{
    public static class PluginLoaderUseExtensions
    {
        internal static readonly string IModuleEntryTypeName = typeof(IModuleEntry).FullName;

        public static IEnumerable<Assembly> Load(this IPluginLoader loader, string searchPath)
        {
            return loader.Load(searchPath, SearchOption.AllDirectories);
        }
        public static IEnumerable<Assembly> Load(this IPluginLoader loader, string searchPath, SearchOption option)
        {
            return Load(loader, DependencySearcher.SearchDeps(searchPath, option), false, false);
        }
        public static IEnumerable<Assembly> Load(this IPluginLoader loader, string searchPath, SearchOption option, bool loadPdb)
        {
            return Load(loader, DependencySearcher.SearchDeps(searchPath, option), false, loadPdb);
        }
        public static IEnumerable<Assembly> Load(this IPluginLoader loader, IEnumerable<string> deps)
        {
            return Load(loader, deps, false, false);
        }
        public static IEnumerable<Assembly> Load(this IPluginLoader loader, IEnumerable<string> deps, bool throwIfFileNotFound)
        {
            return Load(loader, deps, throwIfFileNotFound, false);
        }
        public static IEnumerable<Assembly> Load(this IPluginLoader loader, IEnumerable<string> deps, bool throwIfFileNotFound,bool loadPdb)
        {
            foreach (var item in deps)
            {
                var d = item.Replace(DependencySearcher.DependencyExtensionsName, "dll");
                if (File.Exists(d))
                {
                    yield return AssemblyLoader.LoadFromAssemblyPath(loader, d, loadPdb);
                }
                else if (throwIfFileNotFound)
                {
                    throw new FileNotFoundException(item);
                }
            }
        }
        public static IEnumerable<Type> FindModuleTypes(this IPluginLoader loader)
        {
            if (loader is null)
            {
                throw new ArgumentNullException(nameof(loader));
            }
            return AssemblyModuleContext.GetModuleType(loader.Assemblies);
        }
    }
}
