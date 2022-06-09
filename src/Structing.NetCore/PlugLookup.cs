using McMaster.NETCore.Plugins;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Structing.NetCore
{
    public class PlugLookup : List<PluginLoader>
    {
        public static readonly string ModuleTypeName = typeof(IModuleEntry).FullName;

        private static readonly Type[] moduleTypes = new Type[] { typeof(IModuleEntry) };

        public PluginLoader AddFile(string path)
        {
            return AddFile(path, null);
        }
        public PluginLoader AddFile(string path, Action<PluginConfig> configure)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }
            var loader = PluginLoader.CreateFromAssemblyFile(
               path,
               sharedTypes: moduleTypes,
               configure);
            Add(loader);
            return loader;
        }

        public ModuleCollection Build()
        {
            var coll = new ModuleCollection();
            foreach (var item in this)
            {
                var types = item.LoadDefaultAssembly()
                            .GetExportedTypes()
                            .Where(x => x.GetInterface(ModuleTypeName) != null && !x.IsAbstract && x.GetConstructor(Type.EmptyTypes) != null);
                foreach (var type in types)
                {
                    coll.Add(CreateEntry(type));
                }
            }
            return coll;
        }
        protected virtual IModuleEntry CreateEntry(Type type)
        {
            return (IModuleEntry)Activator.CreateInstance(type);
        }
    }
}
