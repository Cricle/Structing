using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Structing.Core
{
    public class ModuleInfo : Dictionary<string, object>, IModuleInfo
    {
        private static ModuleInfo Build(ModuleInfo info, FileVersionInfo verinfo)
        {
            info["IsPreRelease"] = verinfo.IsPreRelease;
            info["ProductPrivatePart"] = verinfo.ProductPrivatePart;
            info["ProductName"] = verinfo.ProductName;
            info["ProductMinorPart"] = verinfo.ProductMinorPart;
            info["ProductMajorPart"] = verinfo.ProductMajorPart;
            info["ProductBuildPart"] = verinfo.ProductBuildPart;
            info["PrivateBuild"] = verinfo.PrivateBuild;
            info["OriginalFilename"] = verinfo.OriginalFilename;
            info["LegalTrademarks"] = verinfo.LegalTrademarks;
            info["LegalCopyright"] = verinfo.LegalCopyright;
            info["Language"] = verinfo.Language;
            info["IsSpecialBuild"] = verinfo.IsSpecialBuild;
            info["IsPrivateBuild"] = verinfo.IsPrivateBuild;
            info["ProductVersion"] = verinfo.ProductVersion;
            info["SpecialBuild"] = verinfo.SpecialBuild;
            info["IsDebug"] = verinfo.IsDebug;
            info["InternalName"] = verinfo.InternalName;
            info["FileVersion"] = verinfo.FileVersion;
            info["FilePrivatePart"] = verinfo.FilePrivatePart;
            info["FileName"] = verinfo.FileName;
            info["FileMinorPart"] = verinfo.FileMinorPart;
            info["FileMajorPart"] = verinfo.FileMajorPart;
            info["FileDescription"] = verinfo.FileDescription;
            info["FileBuildPart"] = verinfo.FileBuildPart;
            info["CompanyName"] = verinfo.CompanyName;
            info["Comments"] = verinfo.Comments;
            info["IsPatched"] = verinfo.IsPatched;
            return info;
        }

        public static ModuleInfo FromAssembly(Assembly assembly)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return Build(new ModuleInfo(), versionInfo);
        }
    }
}
