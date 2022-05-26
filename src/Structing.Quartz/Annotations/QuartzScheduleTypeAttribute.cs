using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class QuartzScheduleTypeAttribute : Attribute
    {
        public QuartzScheduleTypeAttribute(QuartzScheduleTypes scheduleType)
        {
            ScheduleType = scheduleType;
        }

        public QuartzScheduleTypes ScheduleType { get; }
    }
}
