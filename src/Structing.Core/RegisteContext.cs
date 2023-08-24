using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace Structing
{
    public class RegisteContext : IRegisteContext
    {
        public RegisteContext(IServiceCollection services, IDictionary features = null)
        {
            Services = services ?? throw new System.ArgumentNullException(nameof(services));
            Features = features ?? new Dictionary<object, object>();
        }

        public IServiceCollection Services { get; }

        public IDictionary Features { get; }
    }
}
