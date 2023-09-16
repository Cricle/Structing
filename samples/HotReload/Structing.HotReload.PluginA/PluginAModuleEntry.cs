using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using Structing.HotReload.Contract;

namespace Structing.HotReload.PluginA
{
    public class PluginAModuleEntry : AutoModuleEntry
	{
        public override void Register(IRegisteContext context)
        {
            base.Register(context);
            context.Services.AddSingleton<ISayer, PluginSayer>();
        }
	}
}
