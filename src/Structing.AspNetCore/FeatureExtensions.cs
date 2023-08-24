using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Structing.Annotations;

namespace Structing.AspNetCore
{
    [Feature(ApplicationPartManagerKey,Type =typeof(ApplicationPartManager))]
    [Feature(WebApplicationBuilderKey, Type = typeof(WebApplicationBuilder))]
    [Feature(ApplicationBuilderKey, Type = typeof(IApplicationBuilder))]
    [Feature(HostKey, Type = typeof(IHost))]
    [Feature(EndpointRouteBuilderKey, Type = typeof(IEndpointRouteBuilder))]
    [Feature(WebApplicationKey, Type = typeof(WebApplication))]
    [Feature(ConfigurationKey, Type = typeof(IConfiguration))]
    public static class FeatureExtensions
    {
        public const string WebApplicationBuilderKey = "Structing.Web.WebApplicationBuilder";
        public const string ApplicationPartManagerKey = "Structing.Web.ApplicationPartManager";
        public const string ApplicationBuilderKey = "Structing.Web.ApplicationBuilder";
        public const string HostKey = "Structing.Web.Host";
        public const string EndpointRouteBuilderKey = "Structing.Web.EndpointRouteBuilder";
        public const string WebApplicationKey = "Structing.Web.WebApplication";
        public const string ConfigurationKey = "Structing.Web.Configuration";
    }
}
