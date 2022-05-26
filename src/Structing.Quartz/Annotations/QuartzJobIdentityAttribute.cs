using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QuartzJobIdentityAttribute : Attribute
    {
        public string Identity { get; set; }

        public string Group { get; set; }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QuartzDescriptIdentityAttribute : Attribute
    {
        public QuartzDescriptIdentityAttribute(string descript)
        {
            Descript = descript;
        }

        public string Descript { get; }
    }
}
