using System;
using System.IO;
using System.Reflection;

namespace Structing.NetCore
{
    public record class PluginInfo
    {
        public PluginInfo(string directory, string fileName, bool optional)
        {
            Directory = directory;
            FileName = fileName;
            Optional = optional;
            Path = System.IO.Path.Combine(Directory, FileName);
        }

        public string Directory { get; }

        public string FileName { get; }

        public bool Optional { get; }

        public bool Exists => File.Exists(Path);

        public string Path { get; }

        public Func<Assembly, IModuleEntry>? ModuleEntryCreator { get; set; }
    }
}
