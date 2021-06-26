using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace Structing.Core
{
    public interface IRegisteContext
    {
        IServiceCollection Services { get; }
        IDictionary Features { get; }
    }
}
