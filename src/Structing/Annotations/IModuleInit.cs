using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Structing.Core;

namespace Structing.Annotations
{
    /// <summary>
    /// 表示准备的调用
    /// </summary>
    public interface IModuleInit
    {
        /// <summary>
        /// 调用以准备模块
        /// </summary>
        /// <param name="context">准备上下文</param>
        /// <returns>调用任务</returns>
        Task InvokeAsync(IReadyContext context);
    }
}
