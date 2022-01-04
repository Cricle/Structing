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
            return AddFolder(pluginLookup, folderPath, SearchOption.TopDirectoryOnly, null);
        }
        public static IList<PluginLoader> AddFolder(this PlugLookup pluginLookup, string folderPath, Action<PluginConfig> configure)
        {
            return AddFolder(pluginLookup, folderPath, SearchOption.TopDirectoryOnly, configure);
        }
        public static IList<PluginLoader> AddFolder(this PlugLookup pluginLookup, string folderPath, SearchOption searchOption)
        {
            return AddFolder(pluginLookup, folderPath, searchOption, null);
        }
        public static IList<PluginLoader> AddFolder(this PlugLookup pluginLookup,string folderPath, SearchOption searchOption,Action<PluginConfig> configure)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException(folderPath);
            }
            var loaders = new List<PluginLoader>();
            foreach (var item in Directory.EnumerateDirectories(folderPath, "*", searchOption))
            {
                var folderName=Path.GetFileName(item);
                var dll = Path.Combine(item, folderName + ".dll");
                if (File.Exists(dll))
                {
                    loaders.Add(pluginLookup.AddFile(dll, configure));
                }
            }
            return loaders;
        }
    }
}
