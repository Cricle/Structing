using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Assembly,AllowMultiple =true,Inherited = false)]
    public class ModuleEntryAttribute : Attribute
    {
        public ModuleEntryAttribute(Type moduleType)
        {
            ModuleType = moduleType ?? throw new ArgumentNullException(nameof(moduleType));
            if (moduleType.GetInterface(typeof(IModuleEntry).FullName)==null)
            {
                throw new ArgumentException($"Type {moduleType} does not implement interface IModuleEntry!");
            }
        }

        public Type ModuleType { get; }
    }
}
