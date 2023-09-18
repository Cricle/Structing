using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using System;

namespace Structing.HotReload
{
    public interface IProjectCompiledResult : IDisposable
    {
        string ProjectFilePath { get; }

        Project Project { get; }

        MSBuildWorkspace Workspace { get; }
    }
}
