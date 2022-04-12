using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ConfigJobArgumentsAttribute : Attribute
    {
        protected ConfigJobArgumentsAttribute()
        {

        }

        public ConfigJobArgumentsAttribute(params object[] args)
        {
            Args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public object[] Args { get; protected set; }
    }
}
