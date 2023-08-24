using Structing;
using Structing.Annotations;
using System;

namespace Structing.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ModulePartAttribute : ServiceRegisterAttribute
    {
        private static readonly string InterfaceName = typeof(IModulePart).FullName;

        public ActivationPositions Positions { get; set; } = ActivationPositions.Default;

        public int Order { get; set; }

        public bool OnlyCodeGen { get; set; }

        public override void Register(IRegisteContext context, Type type)
        {
            if (OnlyCodeGen)
            {
                return; 
            }
            if (type.GetInterface(InterfaceName) == null ||
                !type.IsClass || type.IsAbstract)
            {
                throw new InvalidOperationException($"Type {type.FullName} must implement {InterfaceName} and must class, not abstract!");
            }
            ((IModulePart)Activator.CreateInstance(type)).Invoke(context);
        }
    }
}
