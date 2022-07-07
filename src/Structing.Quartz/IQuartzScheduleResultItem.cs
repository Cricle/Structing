using Quartz;
using System;
using System.Collections.Generic;

namespace Structing.Quartz
{
    public interface IQuartzScheduleResultItem
    {
        JobKey JobKey { get; }

        IReadOnlyList<TriggerKey> TriggerKeys { get; }

        DateTimeOffset? NextTriggerTime { get; }

        bool BreakInConfigKey { get; }

        bool BreakInConfigTrigger { get; }

        bool BreakInCheckExists { get; }
    }
}
