using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Structing.HotReload
{
    public class PhysicalFileCompileResultEmitter : ICompileResultEmitter
    {
        public PhysicalFileCompileResultEmitter(string basePath)
        {
            BasePath = basePath ?? throw new ArgumentNullException(nameof(basePath));
            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
        }

        public string BasePath { get; }

        public bool EmitPdb { get; set; } = true;

        public bool EmitXml { get; set; }

        public async Task<EmitResult> EmitResultAsync(IProjectCompiledResult result, CancellationToken token = default)
        {
            var folder = Path.Combine(BasePath, result.Project.AssemblyName);
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder,true);
            }
            Directory.CreateDirectory(folder);
            var compilation = await result.Project.GetCompilationAsync(token);
            if (compilation == null)
            {
                throw new InvalidOperationException($"Fail to get {result.ProjectFilePath} compilation");
            }

            var dllPath = Path.Combine(folder, result.Project.AssemblyName + ".dll");
            string? pdbPath = null;
            string? xmlPath = null;
            if (EmitPdb)
            {
                pdbPath = Path.Combine(folder, result.Project.AssemblyName + ".pdb");
            }
            if (EmitXml)
            {
                xmlPath = Path.Combine(folder, result.Project.AssemblyName + ".xml");
            }
            return compilation.Emit(dllPath, pdbPath, xmlPath, cancellationToken: token);
        }
    }
}
