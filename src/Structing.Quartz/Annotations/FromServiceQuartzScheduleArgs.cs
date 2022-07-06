using System;

namespace Structing.Quartz.Annotations
{
    public class FromServiceQuartzScheduleArgs : QuartzScheduleArgsBase
    {
        public Type JobConfigerType { get; set; }
    }
}
