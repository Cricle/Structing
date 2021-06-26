using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Structing.Core.Annotations
{
    public abstract class ServiceRegisterAttribute : Attribute
    {
        public abstract void Register(IRegisteContext context, Type type);
    }
}
