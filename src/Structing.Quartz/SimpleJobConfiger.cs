using Quartz;
using Structing.Quartz.Annotations;
using System;
using System.Threading.Tasks;

namespace Structing.Quartz
{
    public abstract class SimpleJobConfiger : IJobConfiger
    {
        private static readonly Task<ConfigResults> skip = Task.FromResult(ConfigResults.Skip);
        private static readonly Task<ConfigResults> @continue = Task.FromResult(ConfigResults.Continue);

        private static readonly Task complatedTask = Task.FromResult(false);

        public virtual Task ComplatedAsync(IComplatedScheduleJobContext context)
        {
            return complatedTask;
        }

        public Task<ConfigResults> ConfigKeyAsync(IJobKeyScheduleJobContext context)
        {
            if (IsRequestRecovery(context))
            {
                context.JobBuilder.RequestRecovery();
            }
            var identity = GetIdentity(context);
            if (identity != null)
            {
                context.JobBuilder.WithIdentity(identity.Identity, identity.Group);
            }
            var descript = GetDescript(context);
            if (descript != null)
            {
                context.JobBuilder.WithDescription(descript);
            }
            var jobMap = GetJobMap(context);
            if (jobMap != null)
            {
                context.JobBuilder.SetJobData(jobMap);
            }
            return EndConfigKeyAsync(context);
        }
        protected virtual Task<ConfigResults> EndConfigKeyAsync(IJobKeyScheduleJobContext context)
        {
            return @continue;
        }
        protected virtual QuartzJobIdentity GetIdentity(IJobKeyScheduleJobContext context)
        {
            return null;
        }
        protected virtual JobDataMap GetJobMap(IJobKeyScheduleJobContext context)
        {
            return null;
        }
        protected virtual string GetDescript(IJobKeyScheduleJobContext context)
        {
            return null;
        }
        protected virtual bool IsRequestRecovery(IJobKeyScheduleJobContext context)
        {
            return true;
        }
        public Task<ConfigResults> ConfigTriggerAsync(IJobTriggerScheduleJobContext context)
        {
            var schedule = GetSchedule(context);
            if (schedule != null)
            {
                schedule.Schedule(context, new TriggerBuilderBox(context.TriggerBuilder));
            }
            return EndConfigTriggerAsync(context, schedule);
        }
        protected virtual DateTime? GetStartAt(IJobTriggerScheduleJobContext context, IQuartzSchedule schedule)
        {
            return null;
        }
        protected virtual Task<ConfigResults> EndConfigTriggerAsync(IJobTriggerScheduleJobContext context, IQuartzSchedule schedule)
        {
            var startAt = GetStartAt(context, schedule);
            if (startAt != null)
            {
                context.TriggerBuilder.StartAt(startAt.Value);
            }
            return schedule == null ? skip : @continue;
        }
        protected abstract IQuartzSchedule GetSchedule(IJobTriggerScheduleJobContext context);

        public static QuartzSimpleSchedule Simple(TimeSpan interval, int? repeatCount = 1)
        {
            return new QuartzSimpleSchedule
            {
                Interval = interval,
                RepeatCount = (repeatCount ?? 2) - 1,
                RepeatForever = repeatCount == null,
            };
        }

        public static QuartzCronSchedule Cron(string cron)
        {
            return new QuartzCronSchedule
            {
                CronExpression = cron
            };
        }
    }

}
