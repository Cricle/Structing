using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Structing.Core.Annotations;
using Structing.Core;

namespace Structing.Annotations
{
    /// <summary>
    /// 启用服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class EnableServiceAttribute : ServiceRegisterAttribute
    {

        /// <summary>
        /// 获取或设置一个值，表示实现的类型
        /// </summary>
        public Type ImplementType { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示服务类型
        /// </summary>
        public Type ServiceType { get; set; }
        /// <summary>
        /// 获取或设置一个值，表示服务的生命状态
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; set; }
        /// <inheritdoc/>
        public override void Register(IRegisteContext context, Type type)
        {
            var implType = ImplementType ?? type;
            var serviceType = ServiceType ?? type;

            context.Services.Add(new ServiceDescriptor(serviceType, implType, ServiceLifetime));
        }

    }
}
