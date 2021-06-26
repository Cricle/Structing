using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Structing.Core.Annotations;
using Structing.Core;

namespace Structing.Annotations
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class EnableServiceAttribute : ServiceRegisterAttribute
    {
        public Type ImplementType { get; set; }

        public Type ServiceType { get; set; }
        
        public ServiceLifetime ServiceLifetime { get; set; }

        public override void Register(IRegisteContext context, Type type)
        {
            var implType = ImplementType ?? type;
            var serviceType = ServiceType ?? type;

            context.Services.Add(new ServiceDescriptor(serviceType, implType, ServiceLifetime));
        }

    }
}
