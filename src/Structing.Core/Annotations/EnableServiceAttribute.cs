using Microsoft.Extensions.DependencyInjection;
using Structing.Core;
using Structing.Core.Annotations;
using System;

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
