using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Structing.Core
{
    public class ReadyContext : IReadyContext
    {
        public ReadyContext(IServiceProvider provider, IConfiguration configuration, IDictionary features=null)
        {
            Provider = provider;
            Configuration = configuration;
            Features = features ?? new Dictionary<object, object>();
        }
        public ReadyContext(IServiceProvider provider, IDictionary features = null)
            :this(provider,provider.GetService(typeof(IConfiguration)) as IConfiguration, features)
        {
        }

        public IServiceProvider Provider { get; }

        public IConfiguration Configuration { get; }

        public IDictionary Features { get; }

        public object GetService(Type serviceType)
        {
            return Provider.GetService(serviceType);
        }
    }
}
