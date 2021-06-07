using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Structing.Core
{
    /// <summary>
    /// 准备上下文
    /// </summary>
    public interface IReadyContext:IServiceProvider
    {
        /// <summary>
        /// 服务提供者
        /// </summary>
        IServiceProvider Provider { get; }
        /// <summary>
        /// 配置器
        /// </summary>
        IConfiguration Configuration { get; }
        /// <summary>
        /// 特性
        /// </summary>
        IDictionary Features { get; }
    }
}
