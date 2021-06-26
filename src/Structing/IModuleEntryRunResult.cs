using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using Structing.Core;
using Microsoft.Extensions.Configuration;
using System;

namespace Structing
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
