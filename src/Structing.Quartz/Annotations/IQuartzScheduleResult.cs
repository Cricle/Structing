using Quartz;
using System.Collections.Generic;

namespace Structing.Quartz.Annotations
{
    public interface IQuartzScheduleResult
    {
        ISchedulerFactory SchedulerFactory { get; }

        IScheduler Scheduler { get; }

        IJobConfiger JobConfiger { get; }

        IReadOnlyList<object> Args { get; }

        IReadOnlyList<IQuartzScheduleResultItem> Items { get; }
    }
}
