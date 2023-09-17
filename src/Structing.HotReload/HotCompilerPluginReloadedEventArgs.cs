using System;

namespace Structing.HotReload
{
    public class HotCompilerPluginReloadedEventArgs : EventArgs
    {
        public HotCompilerPluginReloadedEventArgs(HotCompiler compiler, IServiceProvider serviceProvider)
        {
            Compiler = compiler;
            ServiceProvider = serviceProvider;
        }

        public HotCompiler Compiler { get; }

        public IServiceProvider ServiceProvider { get; }
    }
}
