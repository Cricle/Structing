using Quartz;
using Structing.Quartz.Annotations;
using System;

namespace Structing.Quartz
{
    public class QuartzCronSchedule : QuartzScheduleBase
    {
        public string CronExpression { get; set; }

        public TimeZoneInfo TimeZone { get; set; }

        public QuartzCronMisfireHandlingTypes? MisfireHandling { get; set; }

        public override void Schedule(IJobTriggerScheduleJobContext context, in TriggerBuilderBox builderBox)
        {
            if (string.IsNullOrEmpty(CronExpression))
            {
                throw new ArgumentException("CronExpression can't be null or empty");
            }
            With(builderBox.Builder);
            builderBox.Builder.WithCronSchedule(CronExpression, x =>
            {
                if (TimeZone != null)
                {
                    x.InTimeZone(TimeZone);
                }
                if (MisfireHandling != null)
                {
                    switch (MisfireHandling.Value)
                    {
                        case QuartzCronMisfireHandlingTypes.IgnoreMisfires:
                            x.WithMisfireHandlingInstructionIgnoreMisfires();
                            break;
                        case QuartzCronMisfireHandlingTypes.FireAndProceed:
                            x.WithMisfireHandlingInstructionFireAndProceed();
                            break;
                        case QuartzCronMisfireHandlingTypes.DoNothing:
                            x.WithMisfireHandlingInstructionDoNothing();
                            break;
                        default:
                            break;
                    }
                }
            });
        }
    }
}
