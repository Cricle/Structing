using System;

namespace Structing.NetCore
{
    [Flags]
    public enum PluginReloadMode
    {
        None = 0,
        Build = 1,
        Run = Build << 1,
        All=int.MaxValue
    }
}
