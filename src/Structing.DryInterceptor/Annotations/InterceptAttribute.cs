using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Collections;

namespace Structing.DryInterceptor.Annotations
{
    public class InterceptAttribute : ServiceRegisterAttribute
    {
        public InterceptAttribute()
        {
        }

        public InterceptAttribute(Type[] interceptionTypes)
        {
            InterceptionTypes = interceptionTypes;
        }

        public InterceptAttribute(Type serviceType, Type[] interceptionTypes)
        {
            ServiceType = serviceType;
            InterceptionTypes = interceptionTypes;
        }

        public Type ServiceType { get; set; }

        public Type[] InterceptionTypes { get; set; }

        public override void Register(IRegisteContext context, Type type)
        {
            if (InterceptionTypes == null || InterceptionTypes.Length == 0)
            {
                return;
            }
            var selectType = ServiceType ?? type;
            var mgr = (InterceptionManager)context.Features[InterceptionManager.FeatureKey];
            foreach (var item in InterceptionTypes)
            {
                mgr.Add(new InterceptionEntity(selectType, item));
            }
        }
    }
}