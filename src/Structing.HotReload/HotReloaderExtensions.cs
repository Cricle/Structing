using System.Collections.Generic;
using System.IO;

namespace Structing.HotReload
{
    public static class HotReloaderExtensions
    {
        public static IHotReloader AddCSProjs(this IHotReloader hotReloader,string basePath,IEnumerable<string> folders)
        {
            foreach (var item in folders)
            {
                var isRootPath = Path.IsPathRooted(item);
                var relative = Path.Combine(item, $"{item}.csproj");
                var path = isRootPath? item : Path.Combine(basePath, relative);
                if (File.Exists(path))
                {
                    hotReloader.Add(path);
                }
            }
            return hotReloader;
        }
    }
}