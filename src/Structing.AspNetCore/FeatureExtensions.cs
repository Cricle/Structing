using Microsoft.AspNetCore.Builder;
using System;
using System.Collections;

namespace Structing.AspNetCore
{
    public static class FeatureExtensions
    {
        public static readonly string ApplicationBuilderKey = "Structing.Web.ApplicationBuilder";
        public static readonly string ServicePickerKey = "Structing.Web.ServicePicker";
        public static readonly string ApplicationPartManagerKey = "Structing.Web.ApplicationPartManager";

        public static T Get<T>(this IDictionary map, string key)
        {
            if (map.Contains(key))
            {
                return (T)map[key];
            }
            return default;

        }
        public static T EnsureGet<T>(this IDictionary map, string key)
        {
            var val = map.Get<T>(key);
            if (val == null)
            {
                throw new ArgumentException($"Key {key} not found in map");
            }
            return val;
        }
        public static IApplicationBuilder GetApplicationBuilder(this IDictionary ctx)
        {
            return ctx.EnsureGet<IApplicationBuilder>(ApplicationBuilderKey);
        }
        public static IServicePicker GetServicePicker(this IDictionary ctx)
        {
            return ctx.EnsureGet<IServicePicker>(ServicePickerKey);
        }
        public static ApplicationPartManager GetApplicationPartManager(this IDictionary ctx)
        {
            return ctx.EnsureGet<ApplicationPartManager>(ApplicationPartManagerKey);
        }
    }
}
