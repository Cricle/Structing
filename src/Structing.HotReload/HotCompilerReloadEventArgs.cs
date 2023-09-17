using System;

namespace Structing.HotReload
{
    public class HotCompilerReloadEventArgs : EventArgs
    {
        public HotCompilerReloadEventArgs(HotCompiler compiler, bool isFirst)
        {
            Compiler = compiler;
            IsFirst = isFirst;
        }

        public HotCompiler Compiler { get; }

        public bool IsFirst { get; }
    }
}
