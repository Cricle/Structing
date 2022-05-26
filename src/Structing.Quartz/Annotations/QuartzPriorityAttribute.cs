using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QuartzPriorityAttribute : Attribute
    {
        public QuartzPriorityAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}
