using System;

namespace Structing.Quartz.Annotations
{
    [Flags]
    public enum QuartzScheduleTypes
    {
        Simple = 0,
        Cron = 1
    }
}
