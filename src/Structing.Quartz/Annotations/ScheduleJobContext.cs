using Quartz;
using Structing.Core;
using System;
using System.Collections.Generic;

namespace Structing.Quartz.Annotations
{
    internal class ScheduleJobContext : IScheduleJobContext, IJobKeyScheduleJobContext, IJobTriggerScheduleJobContext, IComplatedScheduleJobContext
    {
        public List<ITrigger> Triggers { get; } = new List<ITrigger>();

        public Type JobType { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public Type TargetType { get; set; }

        public IScheduler Scheduler { get; set; }

        public IJobConfiger Configer { get; set; }

        public object Arg { get; set; }

        public JobBuilder JobBuilder { get; set; }

        public IJobDetail JobDetail { get; set; }

        public TriggerBuilder TriggerBuilder { get; set; }

        public ITrigger Trigger { get; set; }

        public DateTimeOffset? NextTriggerTime { get; set; }

        public void AddTrigger(ITrigger trigger)
        {
            Triggers.Add(trigger);
        }
    }
}
