using Quartz;
using System;

namespace Structing.Quartz.Annotations
{
    public interface IComplatedScheduleJobContext : IScheduleJobContext
    {
        IJobDetail JobDetail { get; }

        ITrigger Trigger { get; }

        DateTimeOffset? NextTriggerTime { get; }
    }
}
