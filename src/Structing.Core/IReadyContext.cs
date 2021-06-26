using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Structing.Core
{
    public interface IReadyContext:IServiceProvider
    {
        IServiceProvider Provider { get; }
        IConfiguration Configuration { get; }
        IDictionary Features { get; }
    }
}
