using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Idempotent.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class SkipKeyPartAttribute : Attribute
    {
    }
}
