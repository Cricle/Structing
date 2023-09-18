using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using NuGet.Configuration;
using Structing.HotReload.Exceptions;
using System;
using System.IO;
using System.Linq;
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

        private EmitResult Emit(string folder, string assemblyName, Compilation compilation, CancellationToken token)
        {
            var dllPath = Path.Combine(folder, assemblyName + ".dll");
            string? pdbPath = null;
            string? xmlPath = null;
            if (EmitPdb)
            {
                pdbPath = Path.Combine(folder, assemblyName + ".pdb");
            }
            if (EmitXml)
            {
                xmlPath = Path.Combine(folder, assemblyName + ".xml");
            }
            return compilation.Emit(dllPath, pdbPath, xmlPath, cancellationToken: token);
        }

        public async Task<EmitResult> EmitResultAsync(IProjectCompiledResult result, CancellationToken token = default)
        {
            var folder = Path.Combine(BasePath, result.Project.AssemblyName);
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
            Directory.CreateDirectory(folder);
            var compilation = await result.Project.GetCompilationAsync(token);
            if (compilation == null)
            {
                throw new InvalidOperationException($"Fail to get {result.ProjectFilePath} compilation");
            }
            var emitResult = Emit(folder, result.Project.AssemblyName, compilation, token);
            if (!emitResult.Success)
            {
                throw new HotCompileException(result, emitResult, compilation, folder);
            }
            var compiledRef = compilation.References.OfType<CompilationReference>().ToList();
            foreach (var item in compiledRef)
            {
                Emit(folder, item.Compilation.AssemblyName!, item.Compilation, token);
            }
            var nugetFolder = SettingsUtility.GetGlobalPackagesFolder(Settings.LoadDefaultSettings(AppContext.BaseDirectory));
            var nugetRefs = compilation.References.OfType<PortableExecutableReference>().Where(x => x.FilePath!.StartsWith(nugetFolder, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var item in nugetRefs)
            {
                var fn = Path.GetFileName(item.FilePath)!;
                var path = Path.Combine(folder, fn);
                File.Copy(item.FilePath!, path, true);
            }
            return emitResult;
        }
    }
}
