using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Structing.Annotations;
using Structing.Quartz.Annotations;
using System;
using System.Threading.Tasks;

namespace Structing.Quartz.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(x => x.AddConsole());
            services.AddQuartz(x =>
            {
                x.UseMicrosoftDependencyInjectionJobFactory();
            });
            Run(services).GetAwaiter().GetResult();
        }
        private static async Task Run(IServiceCollection services)
        {
            var module = new ThisModuleEntry(typeof(Program).Assembly);
            var result = await module.RunAsync(services);
            var scheduler = await result.GetRequiredService<ISchedulerFactory>().GetScheduler();
            if (!scheduler.IsStarted)
            {
                await scheduler.Start();
            }
            Console.ReadLine();
        }
    }

    [ConfigJob(typeof(SayHelloJob))]
    public class SayHelloJobConfiger : SimpleJobConfiger
    {
        protected override JobDataMap GetJobMap(IJobKeyScheduleJobContext context)
        {
            return new JobDataMap
            {
                ["name"] = "aaa"
            };
        }

        protected override IQuartzSchedule GetSchedule(IJobTriggerScheduleJobContext context)
        {
            return Simple(TimeSpan.FromSeconds(1), 5);
        }
    }
    [SimpleConfigJob]
    [QuartzRepeatCount(5)]
    [PersistJobDataAfterExecution]
    [QuartzDelayStart(TimeTypes.Second, 2)]
    [QuartzJobData("name", "bbb")]
    [QuartzInterval(TimeTypes.Second, 1)]
    [QuartzScheduleType(QuartzScheduleTypes.Simple)]
    [EnableService(ServiceLifetime = ServiceLifetime.Scoped)]
    internal class SayHelloJob : IJob
    {
        private readonly ILogger<SayHelloJob> logger;

        public SayHelloJob(ILogger<SayHelloJob> logger)
        {
            this.logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var count = 1;
            if (context.JobDetail.JobDataMap.TryGetValue<int>("count", out var v))
            {
                count = v;
            }

            logger.LogInformation("Hello {0} hit {1} time {2}",
            context.MergedJobDataMap.Get("name"),
                count,
                context.FireTimeUtc);
            context.JobDetail.JobDataMap.Put("count", count + 1);
            return Task.CompletedTask;
        }
    }
}