using Quartz;
using Structing.Quartz.Annotations;
using System;

namespace Structing.Quartz
{
    public class QuartzSimpleSchedule : QuartzScheduleBase
    {
        public TimeSpan Interval { get; set; }

        public int RepeatCount { get; set; } = 1;

        public bool RepeatForever { get; set; }

        public QuartzSimpleMisfireHandlingTypes? MisfireHandling { get; set; }

        public override void Schedule(IJobTriggerScheduleJobContext context, in TriggerBuilderBox builderBox)
        {
            if (Interval.Ticks == 0)
            {
                throw new ArgumentException("Interval must not 0");
            }
            With(builderBox.Builder);
            builderBox.Builder.WithSimpleSchedule((x =>
            {
                x.WithInterval(Interval);
                if (RepeatForever)
                {
                    x.RepeatForever();
                }
                else
                {
                    if (RepeatCount <= 0)
                    {
                        throw new ArgumentException("WithRepeatCount must more than 0");
                    }
                    x.WithRepeatCount(RepeatCount);
                }
                if (MisfireHandling != null)
                {
                    switch (MisfireHandling.Value)
                    {
                        case Quartz.QuartzSimpleMisfireHandlingTypes.IgnoreMisfires:
                            x.WithMisfireHandlingInstructionIgnoreMisfires();
                            break;
                        case Quartz.QuartzSimpleMisfireHandlingTypes.FireNow:
                            x.WithMisfireHandlingInstructionFireNow();
                            break;
                        case Quartz.QuartzSimpleMisfireHandlingTypes.NowWithExistingCount:
                            x.WithMisfireHandlingInstructionNowWithExistingCount();
                            break;
                        case Quartz.QuartzSimpleMisfireHandlingTypes.NowWithRemainingCount:
                            x.WithMisfireHandlingInstructionNowWithRemainingCount();
                            break;
                        case Quartz.QuartzSimpleMisfireHandlingTypes.NextWithRemainingCount:
                            x.WithMisfireHandlingInstructionNextWithRemainingCount();
                            break;
                        case Quartz.QuartzSimpleMisfireHandlingTypes.NextWithExistingCount:
                            x.WithMisfireHandlingInstructionNextWithExistingCount();
                            break;
                        default:
                            break;
                    }
                }
            }));
        }
    }
}
