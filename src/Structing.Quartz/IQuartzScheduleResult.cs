using Quartz;
using Structing.Quartz.Annotations;
using System.Collections.Generic;

namespace Structing.Quartz
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
