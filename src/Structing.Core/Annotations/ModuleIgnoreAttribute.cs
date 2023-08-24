using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Annotations
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false,Inherited =false)]
    public sealed class ModuleIgnoreAttribute:Attribute
    {
    }
}
