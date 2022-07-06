namespace Structing.Quartz.Annotations
{
    public class InstanceQuartzScheduleArgs: QuartzScheduleArgsBase
    {
        public IJobConfiger JobConfiger { get; set; }
    }
}
