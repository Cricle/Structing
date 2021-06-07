using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace Structing.Core
{
    public class RegisteContext : IRegisteContext
    {
        public RegisteContext(IServiceCollection services, IDictionary features=null)
        {
            Services = services;
            Features = features ?? new Dictionary<object, object>();
        }

        public IServiceCollection Services { get; }

        public IDictionary Features { get; }
    }
}
