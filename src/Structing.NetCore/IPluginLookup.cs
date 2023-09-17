using McMaster.NETCore.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Structing.NetCore
{
    public interface IPluginLookup : IList<PluginInfo>
    {
        PluginLookup Add(string path, bool optional = false, Func<Assembly, IModuleEntry>? moduleEntryCreator = null);
        PluginLookupBuildResult Build(PluginLoader loader);
    }
    public static class PluginLookupExtensions
    {
        public static IPluginLookup AddRange(this IPluginLookup lookup,string basePath,IEnumerable<string> dllNames)
        {
            foreach (var item in dllNames)
            {
                lookup.Add(Path.Combine(basePath, item));
            }
            return lookup;
        }
    }
}