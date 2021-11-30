using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if NETCOREAPP||NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Structing.Outsize
{
    public interface IPluginLoader
    {
        IEnumerable<Assembly> Assemblies { get; }

        ISet<string> Folders { get; }

        Assembly LoadFromAssemblyName(AssemblyName assemblyName);
        Assembly LoadFromAssemblyPath(string assemblyPath);
        Assembly LoadFromStream(Stream assembly);
        Assembly LoadFromStream(Stream assembly, Stream assemblySymbols);

        void Unload();
    }
}
