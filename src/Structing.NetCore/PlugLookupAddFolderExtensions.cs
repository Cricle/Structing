using McMaster.NETCore.Plugins;
using System;
using System.Collections.Generic;
using System.IO;

namespace Structing.NetCore
{
    public static class PlugLookupAddFolderExtensions
    {
        public static IList<PluginLoader> AddFolder(this PlugLookup pluginLookup, string folderPath)
        {
            return AddFolder(pluginLookup, folderPath, SearchOption.TopDirectoryOnly, static _ => { });
        }
        public static IList<PluginLoader> AddFolder(this PlugLookup pluginLookup, string folderPath, Action<PluginConfig> configure)
        {
            return AddFolder(pluginLookup, folderPath, SearchOption.TopDirectoryOnly, configure);
        }
        public static IList<PluginLoader> AddFolder(this PlugLookup pluginLookup, string folderPath, SearchOption searchOption)
        {
            return AddFolder(pluginLookup, folderPath, searchOption, static _ => { });
        }
        public static IList<PluginLoader> AddFolder(this PlugLookup pluginLookup, string folderPath, SearchOption searchOption, Action<PluginConfig> configure)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException(folderPath);
            }
            var loaders = new List<PluginLoader>();
            foreach (var item in Directory.EnumerateFiles(folderPath, "*.deps.json", searchOption))
            {
                var n = Path.GetFileName(item);
                var fn = n.Substring(0, n.Length - 10);
                var dllName = Path.Combine(Path.GetDirectoryName(item), fn + ".dll");
                if (File.Exists(dllName))
                {
                    pluginLookup.AddFile(dllName, configure);
                }
            }

            return loaders;
        }
    }
}
