using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace Structing.AspNetCore
{
    public static class MapExtensions
    {
        public static readonly string MvcBuilderKey = "Structing.AspNetCore.MvcBuilder";
        public static readonly string ConfigurationKey = "Structing.AspNetCore.Configuration";

        public static IMvcBuilder GetMvcBuilder(this IDictionary map)
        {
            return map.EnsureGet<IMvcBuilder>(MvcBuilderKey);
        }
        public static IConfiguration GetConfiguration(this IDictionary map)
        {
            return map.EnsureGet<IConfiguration>(ConfigurationKey);
        }
    }
}
