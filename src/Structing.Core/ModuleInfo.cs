using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Structing.Core
{
    public class ModuleInfo : Dictionary<string, object>, IModuleInfo
    {
        public static ModuleInfo FromAssembly(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            var assemblyName = assembly.GetName();
            return new ModuleInfo
            {
                [ModuleInfoConst.NameKey] = assemblyName.Name,
                [ModuleInfoConst.VersionKey] = assemblyName.Version,
                [ModuleInfoConst.CultureKey] = new CultureInfo(assemblyName.CultureName)
            };
        }
    }
}
