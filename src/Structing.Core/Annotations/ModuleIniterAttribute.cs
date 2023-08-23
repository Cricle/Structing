using Microsoft.Extensions.DependencyInjection;
using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModuleIniterAttribute : ReadyModuleAttribute
    {
        private static readonly string InterfaceName = typeof(IModuleInit).FullName;

        public override Task ReadyAsync(IReadyContext context, Type targetType)
        {
            if (targetType.GetInterface(InterfaceName) == null ||
                !targetType.IsClass || targetType.IsAbstract)
            {
                throw new InvalidOperationException($"Type {targetType.FullName} must implement {InterfaceName} and must class, not abstract!");
            }
            var val = CreateModuleInit(context, targetType);
            if (val is null)
            {
                throw new ArgumentException($"When active type {targetType.FullName}, can't inject all argument, please use ModuleInitConstructorAttribute or declare a empty argument construct");
            }
            return val.InvokeAsync(context);
        }
        protected internal virtual IModuleInit CreateModuleInit(IReadyContext context, Type targetType)
        {
            var emptyArgConst = targetType.GetTypeInfo().DeclaredConstructors.FirstOrDefault(x => x.IsPublic && x.GetParameters().Length == 0);
            if (emptyArgConst != null && emptyArgConst.IsPublic)
            {
                return (IModuleInit)CreateInstance(targetType, emptyArgConst, ArrayHelper<object>.Empty());
            }
            var scopeFactory = context.Provider.GetService<IServiceScopeFactory>();
            var typeConsts = targetType.GetTypeInfo().DeclaredConstructors.Where(x => x.IsPublic);
            var selected = typeConsts.FirstOrDefault(x => x.GetCustomAttribute<ModuleInitConstructorAttribute>() != null);
            if (selected != null)
            {
                if (selected.GetParameters().Length == 0)
                {
                    return (IModuleInit)CreateInstance(targetType, selected, ArrayHelper<object>.Empty());
                }
                using (var scope = scopeFactory.CreateScope())
                {
                    return (IModuleInit)Create(scope.ServiceProvider, targetType, selected, selected.GetParameters());
                }
            }
            var sortedConst = typeConsts
                .ToDictionary(x => x, x => x.GetParameters())
                .OrderByDescending(x => x.Value.Length);
            using (var scope = scopeFactory.CreateScope())
            {
                foreach (var item in sortedConst)
                {
                    var val = Create(scope.ServiceProvider, targetType, item.Key, item.Value);
                    if (val != null)
                    {
                        return (IModuleInit)val;
                    }
                }
            }
            return null;
        }
        protected internal virtual object CreateInstance(Type type, ConstructorInfo info, object[] par)
        {
            return Activator.CreateInstance(type, par);
        }
        private object Create(IServiceProvider provider, Type type, ConstructorInfo info, ParameterInfo[] pars)
        {
            Debug.Assert(provider != null);
            Debug.Assert(info != null);
            Debug.Assert(pars != null);
            Debug.Assert(pars.Length != 0);

            var c = new object[pars.Length];
            for (int i = 0; i < c.Length; i++)
            {
                var par = pars[i];
                var val = provider.GetService(par.ParameterType);
                if (val is null)
                {
                    if (par.HasDefaultValue)
                    {
                        c[i] = par.DefaultValue;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    c[i] = val;
                }
            }
            return CreateInstance(type, info, c);
        }
    }
}
