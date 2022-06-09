using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Structing.Core
{
    public interface IRegisteContext
    {
        IServiceCollection Services { get; }
        IDictionary Features { get; }
    }
}
