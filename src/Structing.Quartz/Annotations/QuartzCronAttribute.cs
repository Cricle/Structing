using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class QuartzCronAttribute : Attribute
    {
        public QuartzCronAttribute(string cron)
        {
            Cron = cron ?? throw new ArgumentNullException(nameof(cron));
        }

        public string Cron { get; }
    }
}
