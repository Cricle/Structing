using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Structing.Core
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
