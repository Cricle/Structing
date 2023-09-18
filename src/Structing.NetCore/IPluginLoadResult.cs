using System;

namespace Structing.NetCore
{
    public interface IPluginLoadResult : IDisposable
    {
        PluginLookupBuildResult BuildResult { get; }

        IServiceProvider ServiceProvider { get; }
    }
}
