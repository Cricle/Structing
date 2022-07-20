using System;

namespace Structing.DryInterceptor
{
    public class InterceptionEntity
    {
        public InterceptionEntity(Type serviceType, Type interceptorType)
        {
            ServiceType = serviceType;
            InterceptorType = interceptorType;
        }

        public Type ServiceType { get; }

        public Type InterceptorType { get; }

        public override int GetHashCode()
        {
            return ServiceType.GetHashCode() ^ InterceptorType.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is InterceptionEntity entity)
            {
                return entity.ServiceType == ServiceType &&
                    entity.InterceptorType == InterceptorType;
            }
            return false;
        }
        public override string ToString()
        {
            return $"{{ServiceType: {ServiceType}, InterceptorType: {InterceptorType}}}";
        }
    }

}
