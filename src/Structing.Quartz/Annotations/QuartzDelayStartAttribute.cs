using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class QuartzDelayStartAttribute : Attribute
    {
        public QuartzDelayStartAttribute(TimeTypes timeType, int count)
        {
            Delay = TimeHelper.GetTime(timeType, count);
        }
        public QuartzDelayStartAttribute(string intervalStr)
        {
            Delay = TimeSpan.Parse(intervalStr);
        }

        public TimeSpan Delay { get; }
    }
}
