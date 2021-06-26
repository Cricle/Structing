using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                if (Count == 0)
                {
                    return 0;
                }
                return (int)this.Average(x => x.Order);
            }
        }

        public bool HasType(Type type)
        {
            return this.Any(x => x.GetType() == type);
        }
        private async Task RunAll<T>(T value, Func<T, IModuleEntry, Task> e)
        {
            Debug.Assert(e != null);
            foreach (var item in this)
            {
                await e(value, item);
            }
        }
        public Task AfterReadyAsync(IReadyContext context)
        {
            return RunAll(context, (a, b) => b.AfterReadyAsync(a));
        }

        public Task BeforeReadyAsync(IReadyContext context)
        {
            return RunAll(context, (a, b) => b.BeforeReadyAsync(a));
        }

        public virtual IModuleInfo GetModuleInfo(IServiceProvider provider)
        {
            throw new NotSupportedException();
        }

        public Task ReadyAsync(IReadyContext context)
        {
            return RunAll(context, (a, b) => b.ReadyAsync(a));
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

        public Task StartAsync(IServiceProvider serviceProvider)
        {
            return RunAll(serviceProvider, (a, b) => b.StartAsync(a));
        }
        public Task StopAsync(IServiceProvider serviceProvider)
        {
            return RunAll(serviceProvider, (a, b) => b.StopAsync(a));
        }
    }
}
