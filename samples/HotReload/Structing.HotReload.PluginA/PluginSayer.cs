using Structing.HotReload.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.HotReload.PluginA
{
    public class PluginSayer : ISayer
    {
        public void Say()
        {
            Console.WriteLine("pluginA hello2");
        }
    }
}
