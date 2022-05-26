using Quartz;
using Structing.Core;
using System;

namespace Structing.Quartz.Annotations
{
    public interface IScheduleJobContext
    {
        Type JobType { get; }

        IReadyContext ReadyContext { get; }

        Type TargetType { get; }

        IScheduler Scheduler { get; }

        IJobConfiger Configer { get; }

        object Arg { get; }
    }
}
