using McMaster.NETCore.Plugins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Structing.NetCore
{
    public interface IPluginLookup : IList<PluginInfo>
    {
        PluginLookup Add(string path, bool optional = false, Func<Assembly, IModuleEntry>? moduleEntryCreator = null);
        PluginLookupBuildResult Build(PluginLoader loader);
    }
}