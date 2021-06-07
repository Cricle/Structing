using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Core
{
    public class ModuleCollection : List<IModuleEntry>, IModuleEntry
    {
        public int Order
        {
            get
            {
                if (Count==0)
                {
                    return 0;
                }
                return (int)this.Average(x => x.Order);
            }
        }

        public bool HasType(Type type)
        {
            return this.Any(x => x.GetType().IsEquivalentTo(type));
        }

        public async Task AfterReadyAsync(IReadyContext context)
        {
            foreach (var item in this)
            {
                await item.AfterReadyAsync(context);
            }
        }

        public async Task BeforeReadyAsync(IReadyContext context)
        {
            foreach (var item in this)
            {
                await item.BeforeReadyAsync(context);
            }
        }

        public IModuleInfo GetModuleInfo(IServiceProvider provider)
        {
            return ModuleInfo.FromAssembly(GetType().Assembly);
        }

        public async Task ReadyAsync(IReadyContext context)
        {
            foreach (var item in this)
            {
                await item.ReadyAsync(context);
            }

        }

        public void ReadyRegister(IRegisteContext context)
        {
            foreach (var item in this)
            {
                item.ReadyRegister(context);
            }

        }

        public void Register(IRegisteContext context)
        {
            foreach (var item in this)
            {
                item.Register(context);
            }
        }

        public async Task StartAsync(IServiceProvider serviceProvider)
        {
            foreach (var item in this)
            {
                await item.StartAsync(serviceProvider);
            }
        }

        public async Task StopAsync(IServiceProvider serviceProvider)
        {
            foreach (var item in this)
            {
                await item.StopAsync(serviceProvider);
            }
        }
    }
}
