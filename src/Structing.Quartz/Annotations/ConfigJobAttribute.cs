using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Quartz.Annotations
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ConfigJobAttribute : ReadyModuleAttribute
    {
        private static readonly object[] emptyArgs = new object[] { null };
        public static readonly string IJobConfigerTypeName = typeof(IJobConfiger).FullName;

        public ConfigJobAttribute(Type jobType)
        {
            JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
        }

        public bool CreateFromProvider { get; set; }

        public Type JobType { get; }

        public bool SkipWhenExists { get; set; } = true;

        public bool Replace { get; set; } = true;

        public override async Task ReadyAsync(IReadyContext context, Type targetType)
        {
            if (targetType.GetInterface(IJobConfigerTypeName) == null)
            {
                throw new ArgumentException($"Type {targetType} does't implement {IJobConfigerTypeName}");
            }
            if (JobType == null)
            {
                throw new InvalidOperationException("JobType is null");
            }
            IJobConfiger configer;
            if (CreateFromProvider)
            {
                configer = (IJobConfiger)context.GetRequiredService(targetType);
            }
            else
            {
                configer = (IJobConfiger)Activator.CreateInstance(targetType);
            }
            var schedulerFactory = context.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            var args = targetType.GetCustomAttribute<ConfigJobArgumentsAttribute>()?.Args ?? emptyArgs;
            foreach (var item in args)
            {
                var ctx = new ScheduleJobContext
                {
                    Arg = item,
                    JobType = JobType,
                    ReadyContext = context,
                    TargetType = targetType,
                    Scheduler = scheduler,
                    Configer = configer
                };
                await ScheduleJobsAsync(ctx);
            }
        }
        private async Task ScheduleJobsAsync(ScheduleJobContext context)
        {
            var jobKeyBuilder = JobBuilder.Create(context.JobType);
            context.JobBuilder = jobKeyBuilder;
            var res = await context.Configer.ConfigKeyAsync(context);
            if (res == ConfigResults.Continue)
            {
                var jobKey = jobKeyBuilder.Build();
                if (SkipWhenExists)
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
                    if (SkipWhenExists)
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
                        await context.Scheduler.ScheduleJob(jobKey, triggers, Replace);
                    }
                    context.Trigger = triggerKey;
                    context.NextTriggerTime = next;
                    await context.Configer.ComplatedAsync(context);
                }
            }
        }
    }
}
