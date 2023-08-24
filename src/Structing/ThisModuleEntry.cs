using Structing;
using System;
using System.Reflection;

namespace Structing
{
    public class ThisModuleEntry : AutoModuleEntry
    {
        public ThisModuleEntry(Assembly assembly,
            Func<IServiceProvider, IModuleInfo> moduleInfoFactory)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            ModuleInfoFactory = moduleInfoFactory;
        }
        public ThisModuleEntry(Assembly assembly)
            : this(assembly, null)
        {
        }

        public Assembly Assembly { get; }

        public Func<IServiceProvider, IModuleInfo> ModuleInfoFactory { get; }

        public override IModuleInfo GetModuleInfo(IServiceProvider provider)
        {
            return ModuleInfoFactory?.Invoke(provider) ?? base.GetModuleInfo(provider);
        }
        protected internal override Assembly GetAssembly()
        {
            return Assembly;
        }
    }
}
