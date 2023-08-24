using Structing;
using Structing.Annotations;
using System;
using System.Threading.Tasks;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ConfigJobAttribute : ReadyModuleAttribute
    {
        public ConfigJobAttribute(Type jobType)
        {
            JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
        }

        public bool CreateFromProvider { get; set; }

        public Type JobType { get; }

        public bool SkipWhenExists { get; set; } = true;

        public bool Replace { get; set; } = true;

        public override Task ReadyAsync(IReadyContext context, Type targetType)
        {
            return QuartzScheduleHelper.ScheduleAsync(context.Provider,
                new FromServiceQuartzScheduleArgs
                {
                    JobType = JobType,
                    JobConfigerType = targetType,
                    Replace = Replace,
                    SkipWhenExists = SkipWhenExists
                });
        }
    }
}
