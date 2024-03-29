﻿using System;
using System.Threading.Tasks;

namespace Structing
{
    public interface IModuleEntry : IModuleReady, IModuleRegister
    {
        int Order { get; }

        IModuleInfo GetModuleInfo(IServiceProvider provider);
        Task StartAsync(IServiceProvider serviceProvider);
        Task StopAsync(IServiceProvider serviceProvider);
    }
}
