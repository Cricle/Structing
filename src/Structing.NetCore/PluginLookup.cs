using McMaster.NETCore.Plugins;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Structing.NetCore
{
    public class PluginLookup : List<PluginInfo>, IPluginLookup
    {
        public static readonly string ModuleTypeName = typeof(IModuleEntry).FullName!;

        public PluginLookup Add(string path, bool optional = false, Func<Assembly, IModuleEntry>? moduleEntryCreator = null)
        {
            var dir = Path.GetDirectoryName(path) ?? string.Empty;
            var fn = Path.GetFileName(path);
            Add(new PluginInfo(dir, fn, optional) { ModuleEntryCreator = moduleEntryCreator });
            return this;
        }
        public PluginLookupBuildResult Build(PluginLoader loader)
        {
            var coll = new ModuleCollection();
            var assemblyMaps = new Dictionary<PluginInfo, Assembly>();
            foreach (var item in this)
            {
                if (item.Exists)
                {
                    var assembly = loader.LoadAssemblyFromPath(item.Path);
                    assemblyMaps[item] = assembly;
                    IModuleEntry? entry = null;
                    if (item.ModuleEntryCreator != null)
                    {
                        entry = item.ModuleEntryCreator(assembly);
                    }
                    else
                    {
                        entry = CreateEntry(assembly);
                    }
                    if (entry != null)
                    {
                        coll.Add(entry);
                    }
                }
            }
            return new PluginLookupBuildResult(coll, assemblyMaps);
        }
        protected virtual IModuleEntry? CreateEntry(Assembly assembly)
        {
            var type = assembly.GetExportedTypes()
                            .Where(x => x.GetInterface(ModuleTypeName) != null)
                            .FirstOrDefault();
            if (type == null)
            {
                return null;
            }
            return (IModuleEntry)Activator.CreateInstance(type)!;
        }
    }
}
