using System.Collections.Generic;

namespace Structing.DryInterceptor
{
    public class InterceptionManager : List<InterceptionEntity>
    {
        public const string FeatureKey = "Features.InterceptManager";
        public static readonly InterceptionManager Instance = new InterceptionManager();

        private InterceptionManager() { }
    }

}
