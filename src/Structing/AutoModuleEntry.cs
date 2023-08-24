using Structing;
using Structing.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Structing
{
    public abstract class AutoModuleEntry : IModuleEntry
    {
        public virtual int Order { get; } = 0;

        public bool Parallel { get; set; }

        public virtual IModuleInfo GetModuleInfo(IServiceProvider provider)
        {
            return ModuleInfo.FromAssembly(GetAssembly());
        }
        public virtual Task ReadyAsync(IReadyContext context)
        {
            return RunAttirbuteAsync<ReadyModuleAttribute>((a, c) => c.ReadyAsync(context, a));
        }
        public virtual void Register(IRegisteContext context)
        {
            RunAttribute<ServiceRegisterAttribute>((a, b) => b.Register(context, a));
        }
        protected void RunAttribute<T>(Action<Type, T> run)
            where T : Attribute
        {
            var attrs = FindType<T>();
            foreach (var item in attrs)
            {
                foreach (var attr in item.Value)
                {
                    run(item.Key, attr);
                }
            }
        }
        protected async Task RunAttirbuteAsync<T>(Func<Type, T, Task> run)
            where T : Attribute
        {
            var attrs = FindType<T>();
            if (attrs.Count != 0)
            {
                List<Task> tasks = null;
                if (Parallel)
                {
                    tasks = new List<Task>();
                }
                foreach (var item in attrs)
                {
                    foreach (var attr in item.Value)
                    {
                        var task = run(item.Key, attr);
                        if (tasks is null)
                        {
                            await task;
                        }
                        else
                        {
                            tasks.Add(task);
                        }
                    }
                }
                if (tasks != null)
                {
                    await Task.WhenAll(tasks);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IReadOnlyDictionary<Type, T[]> FindType<T>()
            where T : Attribute
        {
            var types = GetAssembly().GetTypes();
            return types.ToDictionary(x => x, x => x.GetCustomAttributes<T>().ToArray());
        }
        protected internal virtual Assembly GetAssembly()
        {
            return GetType().Assembly;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Task MakeComplatedTask()
        {
#if NET452
            return Task.FromResult(false);
#else
            return Task.CompletedTask;
#endif
        }
        public virtual Task CloseAsync(IServiceProvider provider)
        {
            return MakeComplatedTask();
        }

        public virtual Task BeforeReadyAsync(IReadyContext context)
        {
            return MakeComplatedTask();
        }

        public virtual Task AfterReadyAsync(IReadyContext context)
        {
            return MakeComplatedTask();
        }

        public virtual Task StartAsync(IServiceProvider provider)
        {
            return MakeComplatedTask();
        }

        public virtual Task StopAsync(IServiceProvider provider)
        {
            return MakeComplatedTask();
        }

        public virtual void ReadyRegister(IRegisteContext context)
        {
        }

        public virtual void AfterRegister(IRegisteContext context)
        {
        }
    }
}
