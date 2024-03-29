﻿using Quartz;
using Structing.Quartz.Annotations;
using System.Collections.Generic;

namespace Structing.Quartz
{
    internal class QuartzScheduleResult : IQuartzScheduleResult
    {
        public ISchedulerFactory SchedulerFactory { get; set; }

        public IScheduler Scheduler { get; set; }

        public IJobConfiger JobConfiger { get; set; }

        public IReadOnlyList<object> Args { get; set; }

        public IReadOnlyList<IQuartzScheduleResultItem> Items { get; set; }
    }
}
