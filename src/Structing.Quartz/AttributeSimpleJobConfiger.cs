using Quartz;
using Structing.Quartz.Annotations;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Quartz
{
    public class AttributeSimpleJobConfiger : SimpleJobConfiger
    {
        protected override Task<ConfigResults> EndConfigTriggerAsync(IJobTriggerScheduleJobContext context, IQuartzSchedule schedule)
        {
            var attr = context.JobType.GetCustomAttribute<QuartzTriggerIdentityAttribute>();
            if (attr != null)
            {
                context.TriggerBuilder.WithIdentity(attr.Identity, attr.Group);
            }
            return base.EndConfigTriggerAsync(context, schedule);
        }
        protected override IQuartzSchedule GetSchedule(IJobTriggerScheduleJobContext context)
        {
            var attr = context.JobType.GetCustomAttribute<QuartzScheduleTypeAttribute>();
            if (attr == null)
            {
                throw new ArgumentException($"Type {context.JobType} doest not has attribute {typeof(QuartzScheduleTypeAttribute)}");
            }
            QuartzScheduleBase schedule = null;
            if (attr.ScheduleType == QuartzScheduleTypes.Cron)
            {
                var cronAttr = context.JobType.GetCustomAttribute<QuartzCronAttribute>();
                if (cronAttr == null)
                {
                    throw new InvalidOperationException($"Type {context.JobType} select {QuartzScheduleTypes.Cron} but not has attribute {typeof(QuartzCronAttribute)}");
                }
                schedule = Cron(cronAttr.Cron);
            }
            else if (attr.ScheduleType == QuartzScheduleTypes.Simple)
            {
                var intervalAttr = context.JobType.GetCustomAttribute<QuartzIntervalAttribute>();
                if (intervalAttr == null)
                {
                    throw new InvalidOperationException($"Type {context.JobType} select {QuartzScheduleTypes.Simple} but not has attribute {typeof(QuartzIntervalAttribute)}");
                }
                int? repeadCount = 1;
                var repeadCountAttr = context.JobType.GetCustomAttribute<QuartzRepeatCountAttribute>();
                if (repeadCountAttr != null)
                {
                    if (repeadCountAttr.Forevery)
                    {
                        repeadCount = null;
                    }
                    else
                    {
                        repeadCount = repeadCountAttr.Count;
                    }
                }
                schedule = Simple(intervalAttr.Interval, repeadCount);

                var delayStartAttr = context.JobType.GetCustomAttribute<QuartzDelayStartAttribute>();
                if (delayStartAttr != null)
                {
                    schedule.StartAt = DateTime.Now.Add(delayStartAttr.Delay);
                }
                var identityAttr = context.JobType.GetCustomAttribute<QuartzJobIdentityAttribute>();
                if (identityAttr != null)
                {
                    schedule.Identity = identityAttr.Identity;
                    schedule.Group = identityAttr.Group;
                }
                var calendarNameAttr = context.JobType.GetCustomAttribute<QuartzCalendarNameAttribute>();
                if (calendarNameAttr != null)
                {
                    schedule.CalendarName = calendarNameAttr.CalendarName;
                }
            }
            else
            {
                throw new NotSupportedException(attr.ScheduleType.ToString());
            }
            var identity = context.JobType.GetCustomAttribute<QuartzJobIdentityAttribute>();
            if (identity != null)
            {
                schedule.Identity = identity.Identity;
                schedule.Group = identity.Group;
            }
            var priorityAttr = context.JobType.GetCustomAttribute<QuartzPriorityAttribute>();
            if (priorityAttr != null)
            {
                schedule.Priority = priorityAttr.Priority;
            }
            var jobDatas = context.JobType.GetCustomAttributes<QuartzJobDataAttribute>();
            if (jobDatas != null)
            {
                IDictionary map = jobDatas.GroupBy(x => x.Name)
                        .ToDictionary(x => x.Key, x => x.Last().Value);
                schedule.JobDataMap = new JobDataMap(map);
            }
            return schedule;
        }
    }

}
