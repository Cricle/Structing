using Structing;
using Structing.Annotations;
using System;

namespace Structing.Annotations
{
    [AttributeUsage(AttributeTargets.Class| AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ModulePartAttribute : ServiceRegisterAttribute
    {
        private static readonly string InterfaceName = typeof(IModulePart).FullName;

        public ActivationPositions Positions { get; set; } = ActivationPositions.Default;

        public int Order { get; set; }

        public override void Register(IRegisteContext context, Type type)
        {
            if (type.GetInterface(InterfaceName) == null ||
                !type.IsClass || type.IsAbstract)
            {
                throw new InvalidOperationException($"Type {type.FullName} must implement {InterfaceName} and must class, not abstract!");
            }
            ((IModulePart)Activator.CreateInstance(type)).Invoke(context);
        }
    }
}
