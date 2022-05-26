using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class QuartzIntervalAttribute : Attribute
    {
        public QuartzIntervalAttribute(TimeTypes timeType, int count)
        {
            Interval = TimeHelper.GetTime(timeType, count);
        }
        public QuartzIntervalAttribute(string intervalStr)
        {
            Interval = TimeSpan.Parse(intervalStr);
        }

        public TimeSpan Interval { get; }
    }
}
