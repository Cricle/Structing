using System;

namespace Structing.Quartz
{
    [Flags]
    public enum QuartzCronMisfireHandlingTypes
    {
        IgnoreMisfires = -1,
        FireAndProceed = 1,
        DoNothing = 2,
    }
}
