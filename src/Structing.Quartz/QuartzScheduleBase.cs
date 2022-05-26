using Quartz;
using Structing.Quartz.Annotations;
using System;

namespace Structing.Quartz
{
    public abstract class QuartzScheduleBase : IQuartzSchedule
    {
        public string Descript { get; set; }

        public string Group { get; set; }

        public string Identity { get; set; }

        public DateTimeOffset? StartAt { get; set; }

        public DateTimeOffset? EndAt { get; set; }

        public int? Priority { get; set; }

        public JobDataMap JobDataMap { get; set; }

        public string CalendarName { get; set; }

        public abstract void Schedule(IJobTriggerScheduleJobContext context, in TriggerBuilderBox builderBox);

        protected void With(TriggerBuilder builder)
        {
            if (!string.IsNullOrEmpty(Descript))
            {
                builder.WithDescription(Identity);
            }
            if (!string.IsNullOrEmpty(Identity))
            {
                if (!string.IsNullOrEmpty(Group))
                {
                    builder.WithIdentity(Identity, Group);
                }
                else
                {
                    builder.WithIdentity(Identity);
                }
            }
            if (StartAt != null)
            {
                builder.StartAt(StartAt.Value);
            }
            builder.EndAt(EndAt);
            if (Priority != null)
            {
                builder.WithPriority(Priority.Value);
            }
            if (JobDataMap != null)
            {
                builder.UsingJobData(JobDataMap);
            }
            if (!string.IsNullOrEmpty(CalendarName))
            {
                builder.ModifiedByCalendar(CalendarName);
            }
        }
    }
}
