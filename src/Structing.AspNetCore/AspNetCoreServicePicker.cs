using Structing.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Structing.AspNetCore
{
    internal class AspNetCoreServicePicker : IServicePicker
    {
        public AspNetCoreServicePicker(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public IWebHostEnvironment Environment { get; }

        public IConfiguration Configuration { get; }

        public string ApplicationName => Environment.ApplicationName;

        public IFileProvider ContentRootFileProvider => Environment.ContentRootFileProvider;

        public string ContentRootPath => Environment.ContentRootPath;

        public string EnvironmentName => Environment.EnvironmentName;

        public string WebRootPath => Environment.WebRootPath;

        public IFileProvider WebRootFileProvider => Environment.WebRootFileProvider;

        public bool IsDevelopment => Environment.IsDevelopment();

        public bool IsProduction => Environment.IsProduction();

        public bool IsStaging => Environment.IsStaging();

    }
}
