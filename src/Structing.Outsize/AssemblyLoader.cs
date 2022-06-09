#if NET5_0_OR_GREATER || NETCOREAPP
using Microsoft.Extensions.DependencyModel;
using System.Runtime.Loader;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Structing.Outsize
{
    public static class AssemblyLoader
    {
        public static Assembly LoadFromAssemblyPath(IPluginLoader loader, string assemblyFullPath, bool loadPdb)
        {
            var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(assemblyFullPath);
            var fileName = Path.GetFileName(assemblyFullPath);
            var directory = Path.GetDirectoryName(assemblyFullPath);

            Assembly assembly = null;

            loader.Folders.Add(directory);

#if NET5_0_OR_GREATER || NETCOREAPP
            var inCompileLibraries = DependencyContext.Default.CompileLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));
            var inRuntimeLibraries = DependencyContext.Default.RuntimeLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));

            if (inCompileLibraries || inRuntimeLibraries)
            {
                assembly = Assembly.Load(new AssemblyName(fileNameWithOutExtension));
            }
            else
            {
                assembly = Load(loader, assemblyFullPath, loadPdb);
            }
#else
            assembly = Load(loader, assemblyFullPath, loadPdb);
#endif

            if (assembly != null)
                LoadReferencedAssemblies(loader, assembly, fileName, directory);

            return assembly;
        }
        private static Assembly Load(IPluginLoader loader, string assemblyFullPath, bool loadPdb)
        {
            if (loadPdb)
            {
                var pdbFile = Path.ChangeExtension(assemblyFullPath, "pdb");

                if (File.Exists(pdbFile))
                {
                    using (var fdll = File.OpenRead(assemblyFullPath))
                    using (var pdll = File.OpenRead(pdbFile))
                    {
                        return loader.LoadFromStream(fdll, pdll);
                    }
                }
            }
            return loader.LoadFromAssemblyPath(assemblyFullPath);
        }

        private static void LoadReferencedAssemblies(IPluginLoader loader, Assembly assembly, string fileName, string directory)
        {
            var filesInDirectory = new HashSet<string>(
                 Directory.GetFiles(directory).Where(x => x != fileName).Select(x => Path.GetFileNameWithoutExtension(x)));
            var references = assembly.GetReferencedAssemblies();

            var len = references.Length;
            for (int i = 0; i < len; i++)
            {
                var reference = references[i];
                var exists = loader.Assemblies.Any(x => x.FullName == reference.FullName);
                if (!exists && filesInDirectory.Contains(reference.Name))
                {
                    var loadFileName = reference.Name + ".dll";
                    var path = Path.Combine(directory, loadFileName);
#if NET5_0_OR_GREATER || NETCOREAPP
                    var loadedAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
#else
                    var loadedAssembly = Assembly.LoadFile(path);
#endif

                    if (loadedAssembly != null)
                        LoadReferencedAssemblies(loader, loadedAssembly, loadFileName, directory);
                }
            }

        }
    }
}
