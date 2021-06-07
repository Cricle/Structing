using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Structing.Core.Annotations
{
    /// <summary>
    /// 表示服务注册的特性
    /// </summary>
    public abstract class ServiceRegisterAttribute : Attribute
    {
        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="context">注册上下文</param>
        /// <param name="type">目标类型</param>
        public abstract void Register(IRegisteContext context, Type type);
    }
}
