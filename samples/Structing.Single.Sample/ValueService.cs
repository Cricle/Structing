using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;

namespace Structing.Single.Sample
{
    [EnableService(ServiceLifetime = ServiceLifetime.Singleton)]
    public class ValueService
    {
        public int Value { get; set; }

        public int GetValue()
        {
            return Value;
        }

    }
}
