using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Structing.Core.Annotations;

namespace Structing.AspNetCore
{
    [Feature(MvcBuilderKey, Type = typeof(IMvcBuilder))]
    [Feature(ConfigurationKey, Type = typeof(IConfiguration))]
    public static class MapExtensions
    {
        public const string MvcBuilderKey = "Structing.AspNetCore.MvcBuilder";
        public const string ConfigurationKey = "Structing.AspNetCore.Configuration";
    }
}
