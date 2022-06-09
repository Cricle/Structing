using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
#if NETCOREAPP || NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Structing.Outsize
{
#if NETCOREAPP||NET5_0_OR_GREATER

    internal class DefaultPluginLoader : IPluginLoader
    {
        public static readonly DefaultPluginLoader Instance = new DefaultPluginLoader();

        private DefaultPluginLoader()
        {
            Folders = new HashSet<string>();
        }

        public ISet<string> Folders { get; }

        public IEnumerable<Assembly> Assemblies => AssemblyLoadContext.Default.Assemblies;

        public Assembly LoadFromAssemblyName(AssemblyName assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName);
        }

        public Assembly LoadFromAssemblyPath(string assemblyPath)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        }

        public Assembly LoadFromStream(Stream assembly)
        {
            return AssemblyLoadContext.Default.LoadFromStream(assembly);
        }

        public Assembly LoadFromStream(Stream assembly, Stream assemblySymbols)
        {
            return AssemblyLoadContext.Default.LoadFromStream(assembly, assemblySymbols);
        }

        public void Unload()
        {
        }
    }

    internal class PluginLoader : AssemblyLoadContext, IPluginLoader
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoader(string pluginPath, bool isCollectible = false)
            : base(isCollectible)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
            Folders = new HashSet<string>();
        }

        public ISet<string> Folders { get; }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
#else
    [Serializable]
    internal class PluginLoader : IPluginLoader
    {
        public AppDomain AppDomain { get; }

        public IEnumerable<Assembly> Assemblies => AppDomain.GetAssemblies();

        public ISet<string> Folders { get; }

        public PluginLoader(AppDomain appDomain)
        {
            AppDomain = appDomain ?? throw new ArgumentNullException(nameof(appDomain));
            AppDomain.AssemblyResolve += OnAppDomainAssemblyResolve;
            Folders = new HashSet<string>();
        }

        private Assembly OnAppDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var dllName = args.Name.Split(',')[0] + ".dll";
            foreach (var item in Folders)
            {
                var file = Path.Combine(item, dllName);
                if (File.Exists(file))
                {
                    return AssemblyLoader.LoadFromAssemblyPath(this, file, true);
                }
            }
            return null;
        }


        public PluginLoader(string name)
            : this(AppDomain.CreateDomain(name))
        {
        }

        public virtual Assembly LoadFromAssemblyName(AssemblyName assemblyName)
        {
            return AppDomain.Load(assemblyName);
        }

        public virtual Assembly LoadFromAssemblyPath(string assemblyPath)
        {
            using (var s = File.OpenRead(assemblyPath))
            {
                return LoadFromStream(s);
            }
        }

        public virtual Assembly LoadFromStream(Stream assembly)
        {
            var buffer = new byte[assembly.Length];
            assembly.Read(buffer, 0, buffer.Length);
            return AppDomain.Load(buffer);
        }

        public virtual Assembly LoadFromStream(Stream assembly, Stream assemblySymbols)
        {
            var buffer = new byte[assembly.Length];
            var bufferSymbols = new byte[assemblySymbols.Length];
            assembly.Read(buffer, 0, buffer.Length);
            assemblySymbols.Read(bufferSymbols, 0, bufferSymbols.Length);
            return LoadFromStream(buffer, bufferSymbols);
        }
        public Assembly LoadFromStream(byte[] assembly, byte[] assemblySymbols)
        {
            return AppDomain.Load(assembly, assemblySymbols);
        }

        public virtual void Unload()
        {
            if (AppDomain != AppDomain.CurrentDomain)
            {
                AppDomain.Unload(AppDomain);
            }
            AppDomain.AssemblyResolve -= OnAppDomainAssemblyResolve;
        }
    }
#endif
}
