using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core.Annotations
{
    /// <summary>
    /// 准备模块特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ReadyModuleAttribute : Attribute
    {
        /// <summary>
        /// 准备模块
        /// </summary>
        /// <param name="context">准备上下文</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public abstract Task ReadyAsync(IReadyContext context,Type targetType);
    }
}
