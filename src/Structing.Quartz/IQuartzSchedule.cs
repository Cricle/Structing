using Quartz;
using Structing.Quartz.Annotations;

namespace Structing.Quartz
{
    public interface IQuartzSchedule
    {
        void Schedule(IJobTriggerScheduleJobContext context,in TriggerBuilderBox builderBox);
    }
}
