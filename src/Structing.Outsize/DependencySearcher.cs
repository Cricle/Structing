using System.Collections.Generic;
using System.IO;
#if NETCOREAPP || NET5_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Structing.Outsize
{
    public static class DependencySearcher
    {
        /// <remarks>
        /// <![CDATA[
        ///     <EnableDynamicLoading>true</EnableDynamicLoading>
        ///     <GenerateRuntimeConfigurationFiles>true</GenerateRuntimeConfigurationFiles>
        /// ]]>
        /// </remarks>    
        public const string DependencyExtensionsName =
#if NET5_0_OR_GREATER || NETCOREAPP
                "deps.json";
#else
                "runtimeconfig.json";
#endif
        public static IEnumerable<string> SearchDeps(string dirPath)
        {
            return SearchDeps(dirPath, SearchOption.AllDirectories);
        }
        public static IEnumerable<string> SearchDeps(string dirPath, SearchOption option)
        {
            return Directory.GetFiles(dirPath,"*." + DependencyExtensionsName, option);
        }
    }
}
