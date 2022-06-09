using System;
using System.Threading.Tasks;

namespace Structing.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ReadyModuleAttribute : Attribute
    {
        public abstract Task ReadyAsync(IReadyContext context, Type targetType);
    }
}
