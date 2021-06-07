using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Structing
{
    /// <summary>
    /// 表示模块实体基类
    /// </summary>
    public abstract class AutoModuleEntity : IModuleEntry
    {
        ///<inheritdoc/>
        public virtual int Order { get; } = 0;

        /// <inheritdoc/>
        public virtual IModuleInfo GetModuleInfo(IServiceProvider provider)
        {
            return ModuleInfo.FromAssembly(GetAssembly());
        }
        /// <inheritdoc/>
        public virtual async Task ReadyAsync(IReadyContext context)
        {
            var types = FindType<ReadyModuleAttribute>();
            foreach (var type in types)
            {
                foreach (var attr in type.Value)
                {
                    await attr.ReadyAsync(context, type.Key);
                }
            }
        }
        /// <inheritdoc/>
        public virtual void Register(IRegisteContext context)
        {
            var types = FindType<ServiceRegisterAttribute>();
            foreach (var type in types)
            {
                foreach (var attr in type.Value)
                {
                    attr.Register(context, type.Key);
                }
            }
        }
        /// <summary>
        /// 调用以寻找类型
        /// </summary>
        /// <typeparam name="TAttr">特性类型</typeparam>
        /// <returns>类型与特性组的映射</returns>
        protected IReadOnlyDictionary<Type, TAttr[]> FindType<TAttr>()
            where TAttr : Attribute
        {
            var types = GetAssembly().GetTypes();
            return types.ToDictionary(x => x, x => x.GetCustomAttributes<TAttr>().ToArray());
        }
        /// <summary>
        /// 调用以获取程序集
        /// </summary>
        /// <returns></returns>
        protected virtual Assembly GetAssembly()
        {
            return GetType().Assembly;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Task MakeComplatedTask()
        {
#if NET45
            var task = new TaskCompletionSource<object>();
            task.SetResult(null);
            return task.Task;
#else
            return Task.CompletedTask;
#endif
        }
        /// <inheritdoc/>
        public virtual Task CloseAsync(IServiceProvider provider)
        {
            return MakeComplatedTask();
        }

        /// <inheritdoc/>
        public virtual Task BeforeReadyAsync(IReadyContext context)
        {
            return MakeComplatedTask();
        }

        /// <inheritdoc/>
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
    }
}
