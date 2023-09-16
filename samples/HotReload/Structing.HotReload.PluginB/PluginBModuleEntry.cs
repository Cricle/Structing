using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;
using Structing.HotReload.Contract;

namespace Structing.HotReload.PluginB
{
    public class PluginBModuleEntry : AutoModuleEntry
	{
        public override void Register(IRegisteContext context)
        {
            base.Register(context);
            context.Services.AddSingleton<ISayer, PluginSayer>();
        }
	}
}
