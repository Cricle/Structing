using Castle.DynamicProxy;
using DryIoc.ImTools;
using Structing.DryInterceptor;
using Structing.DryInterceptor.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DryIoc
{
    public static class DryIocInterception
    {
        private static readonly DefaultProxyBuilder _proxyBuilder = new DefaultProxyBuilder();

        public static InterceptAttribute AutoIntercept(this IRegistrator registrator,Type serviceType, object serviceKey = null)
        {
            var interceptAttr = serviceType.GetCustomAttribute<InterceptAttribute>();
            if (interceptAttr?.InterceptionTypes!=null)
            {
                AutoIntercept(registrator, serviceType, interceptAttr.InterceptionTypes, serviceKey);
            }
            return interceptAttr;
        }
        public static void AutoIntercept(this IRegistrator registrator, Type serviceType, Type[] interceptTypes, object serviceKey = null)
        {
            var asyncInterceptType = typeof(IAsyncInterceptor).FullName;
            foreach (var item in interceptTypes)
            {
                if (item.GetInterface(asyncInterceptType) != null)
                {
                    AsyncIntercept(registrator, serviceType, item, serviceKey);
                }
                else
                {
                    Intercept(registrator, serviceType, item, serviceKey);
                }
            }
        }
        public static void AsyncIntercept<TService,TIntercept>(this IRegistrator registrator,object serviceKey = null)
            where TIntercept:IAsyncInterceptor
            where TService : class
        {
            AsyncIntercept(registrator, typeof(TService), typeof(TIntercept), serviceKey);
        }
        public static void AsyncIntercept(this IRegistrator registrator, Type serviceType, Type interceptorType, object serviceKey = null)
        {
            var t = typeof(AsyncInterceptor<>).MakeGenericType(interceptorType);
            if (!registrator.IsRegistered(t))
            {
                registrator.Register(t);
            }
            Intercept(registrator, serviceType, t, serviceKey);
        }
        public static void Intercept<TService, TIntercept>(this IRegistrator registrator,object serviceKey = null)
            where TIntercept : IInterceptor
            where TService : class
        {
            Intercept(registrator, typeof(TService), typeof(TIntercept), serviceKey);
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
