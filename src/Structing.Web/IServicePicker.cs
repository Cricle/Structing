using Microsoft.Extensions.Configuration;
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
