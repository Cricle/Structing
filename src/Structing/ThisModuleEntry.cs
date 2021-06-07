using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Structing.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Structing
{
    public class ThisModuleEntry : AutoModuleEntity
    {
        public ThisModuleEntry(Assembly assembly,
            Func<IServiceProvider,IModuleInfo> moduleInfoFactory)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            ModuleInfoFactory = moduleInfoFactory;
        }
        public ThisModuleEntry(Assembly assembly)
            :this(assembly,null)
        {
        }

        public Assembly Assembly { get; }

        public Func<IServiceProvider,IModuleInfo> ModuleInfoFactory { get; }

        public override IModuleInfo GetModuleInfo(IServiceProvider provider)
        {
            return ModuleInfoFactory?.Invoke(provider) ?? base.GetModuleInfo(provider);
        }
        protected override Assembly GetAssembly()
        {
            return Assembly;
        }
    }
}
