using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Structing.Core;
using Structing.Core.Annotations;

namespace Structing
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
                [ModuleInfoConst.CultureKey] = assemblyName.CultureInfo
            };
        }
    }
}
