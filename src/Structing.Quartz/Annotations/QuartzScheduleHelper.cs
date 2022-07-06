using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Quartz.Annotations
{
    public static class QuartzScheduleHelper
    {
        private static readonly object[] emptyArgs = new object[] { null };

        public static Task<IQuartzScheduleResult> ScheduleAsync(IServiceProvider provider, FromServiceQuartzScheduleArgs args)
        {
            var inst = (IJobConfiger)provider.GetRequiredService(args.JobConfigerType);
            return ScheduleAsync(provider, inst, args);
        }

        public static Task<IQuartzScheduleResult> ScheduleAsync(IServiceProvider provider, InstanceQuartzScheduleArgs args)
        {
            return ScheduleAsync(provider, args.JobConfiger, args);
        }
        public static async Task<IQuartzScheduleResult> ScheduleAsync(IServiceProvider provider, IJobConfiger jobConfiger, QuartzScheduleArgsBase args)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var schedulerFactory = provider.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();
            var configType = jobConfiger.GetType();
            var inputArgs = configType.GetCustomAttribute<ConfigJobArgumentsAttribute>()?.Args ?? emptyArgs;
            if (args.ConfigJobArgs != null)
            {
                if (args.MergeArgs)
                {
                    if (inputArgs == null)
                    {
                        inputArgs = args.ConfigJobArgs;
                    }
                    else
                    {
                        inputArgs = inputArgs.Concat(args.ConfigJobArgs).ToArray();
                    }
                }
                else
                {
                    inputArgs = args.ConfigJobArgs;
                }
            }
            var items = new List<IQuartzScheduleResultItem>(inputArgs.Length);
            var result = new QuartzScheduleResult
            {
                Args = inputArgs,
                JobConfiger = jobConfiger,
                Scheduler = scheduler,
                SchedulerFactory = schedulerFactory,
                Items = items
            };
            foreach (var item in inputArgs)
            {
                var ctx = new ScheduleJobContext
                {
                    Arg = item,
                    JobType = args.JobType,
                    ServiceProvider = provider,
                    TargetType = configType,
                    Scheduler = scheduler,
                    Configer = jobConfiger,
                };
                var r = await ScheduleJobsAsync(ctx, args.SkipWhenExists, args.Replace);
                items.Add(r);
            }
            return result;
        }


        private static async Task<IQuartzScheduleResultItem> ScheduleJobsAsync(ScheduleJobContext context, bool skipWhenExits, bool replace)
        {
            var jobKeyBuilder = JobBuilder.Create(context.JobType);
            context.JobBuilder = jobKeyBuilder;
            var res = await context.Configer.ConfigKeyAsync(context);
            var item = new QuartzScheduleResultItem();
            if (res == ConfigResults.Continue)
            {
                var jobKey = jobKeyBuilder.Build();
                item.JobKey = jobKey.Key;
                if (skipWhenExits)
                {
                    var exists = await context.Scheduler.CheckExists(jobKey.Key);
                    if (exists)
                    {
                        item.BreakInCheckExists = true;
                        return item;
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
                            item.BreakInConfigTrigger = true;
                            return item;
                        }
                    }
                    DateTimeOffset? next = null;
                    if (context.Triggers.Count == 0)
                    {
                        next = await context.Scheduler.ScheduleJob(jobKey, triggerKey);
                        item.TriggerKeys = new[] { triggerKey.Key };
                        item.NextTriggerTime = next;
                    }
                    else
                    {
                        var triggers = context.Triggers.Concat(new[] { triggerKey }).ToList();
                        item.TriggerKeys = triggers.Select(x => x.Key).ToList();
                        await context.Scheduler.ScheduleJob(jobKey, triggers, replace);
                    }
                    
                    context.Trigger = triggerKey;
                    context.NextTriggerTime = next;
                    await context.Configer.ComplatedAsync(context);
                }
            }

            item.BreakInConfigKey = true;
            return item;
        }

    }
}
