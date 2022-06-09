using Structing.Core;
using Structing.Core.Annotations;
using System;
using System.Collections;

namespace Structing.Web
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class EnableApplicationPartAttribute : ServiceRegisterAttribute
    {
        public override void Register(IRegisteContext context, Type type)
        {
            var mvcBuilder = context.Features.GetApplicationPartManager();
            mvcBuilder.Add(type.Assembly);
        }
    }
}
