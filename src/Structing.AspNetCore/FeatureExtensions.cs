using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Structing.Core.Annotations;

namespace Structing.AspNetCore
{
    [Feature(ApplicationPartManagerKey,Type =typeof(ApplicationPartManager))]
    [Feature(ServicePickerKey, Type = typeof(IServicePicker))]
    [Feature(ApplicationBuilderKey, Type = typeof(IApplicationBuilder))]
    public static class FeatureExtensions
    {
        public const string ApplicationBuilderKey = "Structing.Web.ApplicationBuilder";
        public const string ServicePickerKey = "Structing.Web.ServicePicker";
        public const string ApplicationPartManagerKey = "Structing.Web.ApplicationPartManager";
    }
}
