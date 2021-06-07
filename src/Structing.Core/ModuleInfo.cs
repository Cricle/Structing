using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Structing.Core
{
    /// <summary>
    /// <inheritdoc cref="IModuleInfo"/>
    /// </summary>
    public class ModuleInfo : Dictionary<string, object>, IModuleInfo
    {

        /// <summary>
        /// 调用以从<paramref name="assembly"/>创建<see cref="ModuleInfo"/>
        /// </summary>
        /// <param name="assembly">目标程序集</param>
        /// <returns>模块信息</returns>
        /// <exception cref="ArgumentNullException">抛出当参数<paramref name="assembly"/>为空时</exception>
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
        /// <summary>
        /// 调用以从调用者程序集创建类型<see cref="ModuleInfo"/>
        /// </summary>
        /// <returns>模块信息</returns>
        public static ModuleInfo FromAssembly()
        {
            return FromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
