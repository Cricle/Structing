using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core
{
    /// <summary>
    /// 表示模块入口
    /// </summary>
    public interface IModuleEntry : IModuleReady, IModuleRegister
    {
        /// <summary>
        /// 排序键
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 调用以获取模块信息
        /// </summary>
        /// <param name="provider">服务提供者</param>
        /// <returns></returns>
        IModuleInfo GetModuleInfo(IServiceProvider provider);
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task StartAsync(IServiceProvider serviceProvider);
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="serviceProvider">服务提供者</param>
        /// <returns></returns>
        Task StopAsync(IServiceProvider serviceProvider);
    }
}
