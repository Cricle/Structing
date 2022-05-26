using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QuartzTriggerIdentityAttribute : Attribute
    {
        public string Identity { get; set; }

        public string Group { get; set; }
    }
}
