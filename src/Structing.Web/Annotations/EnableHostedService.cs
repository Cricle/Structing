using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Web.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class EnableHostedServiceAttribute : ServiceRegisterAttribute
    {
        private static readonly string IHostedServiceTypeName = typeof(IHostedService).FullName;

        public override void Register(IRegisteContext context, Type type)
        {
            if (type.GetInterface(IHostedServiceTypeName) == null)
            {
                throw new ArgumentException($"Type {type} is not implement {IHostedServiceTypeName}");
            }
            context.Services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), type));
        }
    }
}
