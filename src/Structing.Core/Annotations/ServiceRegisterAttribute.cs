using System;

namespace Structing.Core.Annotations
{
    public abstract class ServiceRegisterAttribute : Attribute
    {
        public abstract void Register(IRegisteContext context, Type type);
    }
}
