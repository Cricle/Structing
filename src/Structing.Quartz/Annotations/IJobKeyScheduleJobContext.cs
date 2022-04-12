using Quartz;

namespace Structing.Quartz.Annotations
{
    public interface IJobKeyScheduleJobContext : IScheduleJobContext
    {
        JobBuilder JobBuilder { get; }
    }
}
