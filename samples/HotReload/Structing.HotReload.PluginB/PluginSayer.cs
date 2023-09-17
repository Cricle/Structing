using Structing.HotReload.Contract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.HotReload.PluginB
{
    public class PluginSayer : ISayer
    {
        public void Say()
        {
            Console.WriteLine("pluginB hello2");
        }
    }
}
