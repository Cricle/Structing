using System;

namespace Structing.Quartz.Annotations
{
    [Flags]
    public enum TimeTypes
    {
        Milliseconds = 0,
        Second = 1,
        Minute = 2,
        Hour = 3,
        Day = 4,
        Week = 5,
    }
}
