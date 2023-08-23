using Microsoft.Extensions.Configuration;
using System;
using System.Collections;

namespace Structing.Core
{
    public interface IReadyContext : IServiceProvider, IFeatureContext
    {
        IServiceProvider Provider { get; }
        IConfiguration Configuration { get; }
    }
}
