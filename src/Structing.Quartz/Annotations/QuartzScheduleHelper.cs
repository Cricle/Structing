using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Structing.Core;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Quartz.Annotations
{
    public static class QuartzScheduleHelper
    {
        private static readonly object[] emptyArgs = new object[] { null };
        public static readonly string IJobConfigerTypeName = typeof(IJobConfiger).FullName;

        public static async Task ScheduleAsync(IReadyContext context,
            Type jobType,
            Type jobConfigerType,
            bool createFromProvider,
            bool skipWhenExits,
            bool replace)
        {
            if (jobConfigerType.GetInterface(IJobConfigerTypeName) == null)
            {
                throw new ArgumentException($"Type {jobConfigerType} does't implement {IJobConfigerTypeName}");
            }
            if (jobType == null)
            {
                throw new InvalidOperationException("JobType is null");
            }
            IJobConfiger configer;
            if (createFromProvider)
            {
                configer = (IJobConfiger)context.GetRequiredService(jobConfigerType);
            }
            else
            {
                configer = (IJobConfiger)Activator.CreateInstance(jobConfigerType);
            }
            var schedulerFactory = context.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            var args = jobConfigerType.GetCustomAttribute<ConfigJobArgumentsAttribute>()?.Args ?? emptyArgs;
            foreach (var item in args)
            {
                var ctx = new ScheduleJobContext
                {
                    Arg = item,
                    JobType = jobType,
                    ReadyContext = context,
                    TargetType = jobConfigerType,
                    Scheduler = scheduler,
                    Configer = configer
                };
                await ScheduleJobsAsync(ctx, skipWhenExits, replace);
            }
        }
        private static async Task ScheduleJobsAsync(ScheduleJobContext context, bool skipWhenExits, bool replace)
        {
            var jobKeyBuilder = JobBuilder.Create(context.JobType);
            context.JobBuilder = jobKeyBuilder;
            var res = await context.Configer.ConfigKeyAsync(context);
            if (res == ConfigResults.Continue)
            {
                var jobKey = jobKeyBuilder.Build();
                if (skipWhenExits)
                {
                    var exists = await context.Scheduler.CheckExists(jobKey.Key);
                    if (exists)
                    {
                        return;
                    }
                }
                var triggerBuilder = TriggerBuilder.Create();
                context.JobDetail = jobKey;
                context.TriggerBuilder = triggerBuilder;
                res = await context.Configer.ConfigTriggerAsync(context);
                if (res == ConfigResults.Continue)
                {
                    var triggerKey = triggerBuilder.Build();
                    if (skipWhenExits)
                    {
                        var exists = await context.Scheduler.CheckExists(triggerKey.Key);
                        if (exists)
                        {
                            return;
                        }
                    }
                    DateTimeOffset? next = null;
                    if (context.Triggers.Count == 0)
                    {
                        next = await context.Scheduler.ScheduleJob(jobKey, triggerKey);
                    }
                    else
                    {
                        var triggers = context.Triggers.Concat(new[] { triggerKey }).ToList();
                        await context.Scheduler.ScheduleJob(jobKey, triggers, replace);
                    }
                    context.Trigger = triggerKey;
                    context.NextTriggerTime = next;
                    await context.Configer.ComplatedAsync(context);
                }
            }
        }

    }
}
