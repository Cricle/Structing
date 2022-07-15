using Quartz;
using Structing.Quartz.Annotations;
using System;

namespace Structing.Quartz
{
    public class DefaultSimpleJobConfiger : SimpleJobConfiger
    {
        public string Descript { get; set; }

        public QuartzJobIdentity JobIdentity { get; set; }

        public bool RequestRecovery { get; set; }

        public JobDataMap JobDataMap { get; set; }

        public DateTime? StartAt { get; set; }

        public IQuartzSchedule QuartzSchedule { get; set; }

        protected override string GetDescript(IJobKeyScheduleJobContext context)
        {
            return Descript;
        }
        protected override QuartzJobIdentity GetIdentity(IJobKeyScheduleJobContext context)
        {
            return JobIdentity;
        }
        protected override bool IsRequestRecovery(IJobKeyScheduleJobContext context)
        {
            return RequestRecovery;
        }
        protected override JobDataMap GetJobMap(IJobKeyScheduleJobContext context)
        {
            return JobDataMap;
        }
        protected override DateTime? GetStartAt(IJobTriggerScheduleJobContext context, IQuartzSchedule schedule)
        {
            return StartAt;
        }

        protected override IQuartzSchedule GetSchedule(IJobTriggerScheduleJobContext context)
        {
            return QuartzSchedule;
        }
    }

}
