using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class QuartzRepeatCountAttribute : Attribute
    {
        public QuartzRepeatCountAttribute(bool forevery = true)
        {
            Forevery = forevery;
        }

        public QuartzRepeatCountAttribute(int count)
        {
            Count = count;
        }

        public bool Forevery { get; set; }

        public int Count { get; set; }
    }
}
