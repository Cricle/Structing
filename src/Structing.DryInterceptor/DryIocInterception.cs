using Castle.DynamicProxy;
using DryIoc.ImTools;
using Structing.DryInterceptor;
using System;

namespace DryIoc
{
    public static class DryIocInterception
    {
        private static readonly DefaultProxyBuilder _proxyBuilder = new DefaultProxyBuilder();

        public static void AsyncIntercept(this IRegistrator registrator, Type serviceType, Type interceptorType, object serviceKey = null)
        {
            var t = typeof(AsyncInterceptor<>).MakeGenericType(interceptorType);
            registrator.Register(t);
            Intercept(registrator, serviceType, t, serviceKey);
        }

        public static void Intercept(this IRegistrator registrator, Type serviceType, Type interceptorType, object serviceKey = null)
        {
            Type proxyType;
            if (serviceType.IsInterface)
                proxyType = _proxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
                    serviceType, Array.Empty<Type>(), ProxyGenerationOptions.Default);
            else if (serviceType.IsClass)
                proxyType = _proxyBuilder.CreateClassProxyTypeWithTarget(
                    serviceType, Array.Empty<Type>(), ProxyGenerationOptions.Default);
            else
                throw new ArgumentException(
                    $"Intercepted service type {serviceType} is not a supported, cause it is nor a class nor an interface");

            var interceptorsType = interceptorType.MakeArrayType();

            registrator.Register(serviceType, proxyType,
                made: Made.Of(pt => pt.PublicConstructors().FindFirst(ctor => ctor.GetParameters().Length != 0),
                    Parameters.Of.Type<IInterceptor[]>(interceptorsType)),
                setup: Setup.DecoratorOf(useDecorateeReuse: true, decorateeServiceKey: serviceKey));
        }
    }
}
