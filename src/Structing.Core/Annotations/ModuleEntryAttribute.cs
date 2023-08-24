using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Annotations
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class ModuleEntryAttribute : Attribute
    {
    }
}
