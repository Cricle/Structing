using Quartz;
using Structing.Core;
using System;
using System.Threading.Tasks;

namespace Structing.Quartz.Annotations
{
    public interface IJobConfiger
    {
        Task<ConfigResults> ConfigKeyAsync(IJobKeyScheduleJobContext context);

        Task<ConfigResults> ConfigTriggerAsync(IJobTriggerScheduleJobContext context);

        Task ComplatedAsync(IComplatedScheduleJobContext context);
    }
}
