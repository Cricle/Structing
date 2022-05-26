using System;

namespace Structing.Quartz.Annotations
{
    internal static class TimeHelper
    {
        public static TimeSpan GetTime(TimeTypes timeType, int count)
        {
            switch (timeType)
            {
                case TimeTypes.Milliseconds:
                    return TimeSpan.FromMilliseconds(count);
                case TimeTypes.Second:
                    return TimeSpan.FromSeconds(count);
                case TimeTypes.Minute:
                    return TimeSpan.FromMinutes(count);
                case TimeTypes.Hour:
                    return TimeSpan.FromHours(count);
                case TimeTypes.Day:
                    return TimeSpan.FromDays(count);
                case TimeTypes.Week:
                    return TimeSpan.FromDays(count * 7);
                default:
                    throw new NotSupportedException(timeType.ToString());
            }
        }
    }
}
