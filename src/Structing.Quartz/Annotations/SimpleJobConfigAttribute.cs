using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Threading.Tasks;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SimpleConfigJobAttribute : ReadyModuleAttribute
    {
        public bool CreateFromProvider { get; set; }

        public Type JobType { get; set; }

        public bool SkipWhenExists { get; set; } = true;

        public bool Replace { get; set; } = true;

        public Type ConfigType { get; set; } = typeof(AttributeSimpleJobConfiger);

        public override Task ReadyAsync(IReadyContext context, Type targetType)
        {
            var config = new ConfigJobAttribute(JobType ?? targetType)
            {
                CreateFromProvider = CreateFromProvider,
                SkipWhenExists = SkipWhenExists,
                Replace = Replace
            };
            return config.ReadyAsync(context, ConfigType ?? targetType);
        }
    }
}
