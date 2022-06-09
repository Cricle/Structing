using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Structing.Core
{
    public interface IModuleEntryRunResult : IServiceProvider
    {
        IEnumerable<IModuleEntry> ModuleEntries { get; }

        IServiceCollection Services { get; }

        IConfiguration Configuration { get; }

        IDictionary Feature { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
