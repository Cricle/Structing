using System;
using System.IO;
using System.Reflection;

namespace Structing.NetCore
{
    public static class PlugLookupAddFolderExtensions
    {
        public static PluginLookup AddFolder(this PluginLookup pluginLookup,
            string folderPath,
            Func<string, bool>? optionalSelector = null,
            Func<string, Func<Assembly, IModuleEntry>?>? creatorSelector = null)
        {
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException(folderPath);
            }
            foreach (var item in Directory.EnumerateDirectories(folderPath))
            {
                var dllName = Path.GetFileName(item) + ".dll";
                var path = Path.Combine(item, dllName);
                if (File.Exists(path))
                {
                    pluginLookup.Add(path, optionalSelector?.Invoke(path) ?? true, creatorSelector?.Invoke(path));
                }
            }
            return pluginLookup;
        }
    }
}
