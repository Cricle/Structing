using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core
{
    public interface IModuleEntry : IModuleReady, IModuleRegister
    {
        int Order { get; }

        IModuleInfo GetModuleInfo(IServiceProvider provider);
        Task StartAsync(IServiceProvider serviceProvider);
        Task StopAsync(IServiceProvider serviceProvider);
    }
}
