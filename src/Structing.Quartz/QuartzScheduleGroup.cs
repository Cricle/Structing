using Quartz;
using Structing.Quartz.Annotations;
using System.Collections.Generic;

namespace Structing.Quartz
{
    public class QuartzScheduleGroup : List<IQuartzSchedule>, IQuartzSchedule
    {
        public QuartzScheduleGroup()
        {
        }

        public QuartzScheduleGroup(int capacity) : base(capacity)
        {
        }

        public QuartzScheduleGroup(IEnumerable<IQuartzSchedule> collection) : base(collection)
        {
        }

        public void Schedule(IJobTriggerScheduleJobContext context, in TriggerBuilderBox builderBox)
        {
            builderBox.SetIgnore();
            foreach (var item in this)
            {
                var box = new TriggerBuilderBox(TriggerBuilder.Create());
                item.Schedule(context, box);
                if (!box.IsIgnore)
                {
                    context.AddTrigger(box.Builder.Build());
                }
            }
        }
    }
}
