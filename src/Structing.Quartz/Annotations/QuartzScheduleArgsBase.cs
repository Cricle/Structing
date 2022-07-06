using System;

namespace Structing.Quartz.Annotations
{
    public abstract class QuartzScheduleArgsBase
    {
        public Type JobType { get; set; }

        public object[] ConfigJobArgs { get; set; }

        public bool MergeArgs { get; set; } = true;

        public bool SkipWhenExists { get; set; }

        public bool Replace { get; set; }
    }
}
