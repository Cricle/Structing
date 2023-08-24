using Quartz;
using Structing;
using System;

namespace Structing.Quartz.Annotations
{
    public interface IScheduleJobContext
    {
        Type JobType { get; }

        IServiceProvider ServiceProvider { get; }

        Type TargetType { get; }

        IScheduler Scheduler { get; }

        IJobConfiger Configer { get; }

        object Arg { get; }
    }
}
