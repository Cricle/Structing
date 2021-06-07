using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core.Annotations
{
    /// <summary>
    /// 模块初始化器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleIniterAttribute : ReadyModuleAttribute
    {
        private static readonly string InterfaceName = typeof(IModuleInit).FullName;

        /// <inheritdoc/>
        public override Task ReadyAsync(IReadyContext context, Type targetType)
        {
            if (targetType.GetInterface(InterfaceName) == null ||
                !targetType.IsClass || targetType.IsAbstract)
            {
                throw new InvalidOperationException($"Type {targetType.FullName} must implement {InterfaceName} and must class, not abstract!");
            }
            var val = (IModuleInit)Activator.CreateInstance(targetType);
            return val.InvokeAsync(context);
        }
    }
}
