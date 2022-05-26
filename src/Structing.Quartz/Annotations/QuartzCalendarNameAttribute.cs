using System;

namespace Structing.Quartz.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class QuartzCalendarNameAttribute : Attribute
    {
        public QuartzCalendarNameAttribute(string calendarName)
        {
            CalendarName = calendarName ?? throw new ArgumentNullException(nameof(calendarName));
        }

        public string CalendarName { get; }
    }
}
