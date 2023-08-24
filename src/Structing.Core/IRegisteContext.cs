using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Structing
{
    public interface IFeatureContext
    {
        IDictionary Features { get; }
    }

    public interface IRegisteContext: IFeatureContext
    {
        IServiceCollection Services { get; }
    }
}
