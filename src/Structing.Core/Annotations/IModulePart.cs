using Structing.Core;

namespace Structing.Annotations
{
    public interface IModulePart
    {
        void Invoke(IRegisteContext context);
    }
}
