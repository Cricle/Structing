using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Structing.Web
{
    public interface IServicePicker
    {
        IConfiguration Configuration { get; }

        string ApplicationName { get; }

        IFileProvider ContentRootFileProvider { get; }

        string ContentRootPath { get; }

        string EnvironmentName { get; }

        string WebRootPath { get; }

        IFileProvider WebRootFileProvider { get; }

        bool IsDevelopment { get; }

        bool IsProduction { get; }

        bool IsStaging { get; }
    }
}
