﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Structing.HotReload
{
    public readonly record struct ProjectCompiledResult : IProjectCompiledResult
    {
        internal ProjectCompiledResult(MSBuildWorkspace workspace, Project project, string projectFilePath)
        {
            Workspace = workspace;
            Project = project;
            ProjectFilePath = projectFilePath;
        }

        public MSBuildWorkspace Workspace { get; }

        public Project Project { get; }

        public string ProjectFilePath { get; }

        public void Dispose()
        {
            Workspace.Dispose();
        }
    }
}