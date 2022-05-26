using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class QuartzJobDataAttribute : Attribute
    {
        public QuartzJobDataAttribute()
        {
        }

        public QuartzJobDataAttribute(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value { get; set; }
    }
}
