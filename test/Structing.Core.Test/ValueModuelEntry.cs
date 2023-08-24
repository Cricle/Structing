using System;
using System.Threading.Tasks;

namespace Structing.Test
{
    internal class ValueModuelEntry : IModuleEntry
    {
        public int Order { get; set; }

        Task Comaplated()
        {
#if NET5_0
            return Task.CompletedTask;
#else
            return Task.FromResult(0);
#endif
        }
        public bool IsAfterReadyAsync { get; set; }
        public Task AfterReadyAsync(IReadyContext context)
        {
            IsAfterReadyAsync = true;
            return Comaplated();
        }

        public bool IsBeforeReadyAsync { get; set; }
        public Task BeforeReadyAsync(IReadyContext context)
        {
            IsBeforeReadyAsync = true;
            return Comaplated();
        }
        public IModuleInfo Info { get; set; }
        public IModuleInfo GetModuleInfo(IServiceProvider provider)
        {
            return Info;
        }

        public bool IsReadyAsync { get; set; }
        public Task ReadyAsync(IReadyContext context)
        {
            IsReadyAsync = true;
            return Comaplated();
        }

        public bool IsReadyRegister { get; set; }
        public void ReadyRegister(IRegisteContext context)
        {
            IsReadyRegister = true;
        }

        public bool IsRegister { get; set; }
        public void Register(IRegisteContext context)
        {
            IsRegister = true;
        }

        public bool IsStartAsync { get; set; }
        public Task StartAsync(IServiceProvider serviceProvider)
        {
            IsStartAsync = true;
            return Comaplated();
        }

        public bool IsStopAsync { get; set; }
        public Task StopAsync(IServiceProvider serviceProvider)
        {
            IsStopAsync = true;
            return Comaplated();
        }

        public bool IsAfterRegister { get; set; }
        public void AfterRegister(IRegisteContext context)
        {
            IsAfterRegister=true;
        }
    }
}
