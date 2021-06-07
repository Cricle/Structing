using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Core
{
    public static class ModuleCollectionExtensions
    {
        private static readonly string ModuleEntryTypeName = typeof(IModuleEntry).FullName;

        public static async Task RegistAndReadyAsync(this IModuleEntry item,
            IRegisteContext registeContext,Func<IRegisteContext,IReadyContext> readyContextMaker)
        {
            item.RunRegister(registeContext);
            var readyContext = readyContextMaker(registeContext);
            await item.RunReadyAsync(readyContext);
        }

        public static IEnumerable<Type> AddEntryAssembly(this ICollection<IModuleEntry> modules,
            bool single = false)
        {
            return Add(modules, Assembly.GetEntryAssembly(), single);
        }
        public static IEnumerable<Type> Add(this ICollection<IModuleEntry> modules,
           bool single = false)
        {
            return Add(modules, Assembly.GetExecutingAssembly(), single);
        }
        public static IEnumerable<Type> Add(this ICollection<IModuleEntry> modules,
            Assembly assembly,
            bool single = false)
        {
            return Add(modules, assembly, x => (IModuleEntry)Activator.CreateInstance(x), single);
        }
        public static IEnumerable<Type> Add(this ICollection<IModuleEntry> modules,
            Assembly assembly,
            Func<Type, IModuleEntry> typeActivator,
            bool single=false)
        {
            var types = assembly.GetTypes()
                .Where(x => x.GetInterface(ModuleEntryTypeName) != null);
            if (single)
            {
                var type = types.FirstOrDefault();
                if (type!=null)
                {
                    var inst = typeActivator(type);
                    modules.Add(inst);
                    yield return type;
                }
                yield break;
            }
            foreach (var item in types)
            {
                var inst = typeActivator(item);
                modules.Add(inst);
                yield return item;
            }
        }
    }
}
