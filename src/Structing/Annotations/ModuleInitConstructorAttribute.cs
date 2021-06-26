using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Annotations
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class ModuleInitConstructorAttribute : Attribute
    {
    }
}
