using Quartz;

namespace Structing.Quartz.Annotations
{
    public interface IJobTriggerScheduleJobContext : IScheduleJobContext
    {
        IJobDetail JobDetail { get; }

        TriggerBuilder TriggerBuilder { get; }

        void AddTrigger(ITrigger trigger);
    }
}
