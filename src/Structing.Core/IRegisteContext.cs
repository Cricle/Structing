using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Structing.Core
{
    /// <summary>
    /// 注册的上下文
    /// </summary>
    public interface IRegisteContext
    {
        /// <summary>
        /// 获取一个值，表示服务集合
        /// </summary>
        IServiceCollection Services { get; }
        /// <summary>
        /// 特性
        /// </summary>
        IDictionary Features { get; }
    }
}
