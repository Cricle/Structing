using Microsoft.CodeAnalysis.MSBuild;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structing.HotReload
{
    public class DefaultProjectCompiler : IProjectCompiler
    {
        public DefaultProjectCompiler()
        {
        }

        public DefaultProjectCompiler(IProgress<ProjectLoadProgress>? progress)
        {
            Progress = progress;
        }
        public DefaultProjectCompiler(Action<ProjectLoadProgress>? progress)
            : this(progress == null ? null : new Progress<ProjectLoadProgress>(progress))
        {
        }

        public IProgress<ProjectLoadProgress>? Progress { get; set; }

        public async Task<IProjectCompiledResult> CompileAsync(string csprojPath, CancellationToken token = default)
        {
            var dir = MSBuildWorkspace.Create();
            var project = await dir.OpenProjectAsync(csprojPath, Progress, token);
            return new ProjectCompiledResult(dir, project, csprojPath);
        }
    }
}
