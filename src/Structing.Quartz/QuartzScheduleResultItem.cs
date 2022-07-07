using Quartz;
using System;
using System.Collections.Generic;

namespace Structing.Quartz
{
    internal class QuartzScheduleResultItem : IQuartzScheduleResultItem
    {
        public JobKey JobKey { get; set; }

        public IReadOnlyList<TriggerKey> TriggerKeys { get; set; }

        public DateTimeOffset? NextTriggerTime { get; set; }

        public bool BreakInConfigKey { get; set; }

        public bool BreakInConfigTrigger { get; set; }

        public bool BreakInCheckExists { get; set; }
    }
}
