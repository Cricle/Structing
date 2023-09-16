using Microsoft.CodeAnalysis;
using System;

namespace Structing.HotReload
{
    public interface IProjectCompiledResult : IDisposable
    {
        string ProjectFilePath { get; }

        Project Project { get; }
    }
}
