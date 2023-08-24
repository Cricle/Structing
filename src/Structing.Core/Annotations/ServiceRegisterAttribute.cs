using System;

namespace Structing.Annotations
{
    public abstract class ServiceRegisterAttribute : Attribute
    {
        public abstract void Register(IRegisteContext context, Type type);
    }
}
