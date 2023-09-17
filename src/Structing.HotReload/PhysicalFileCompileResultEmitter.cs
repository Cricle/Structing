using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
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

        private EmitResult Emit(string folder,string assemblyName,Compilation compilation,CancellationToken token)
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
            var emitResult=Emit(folder, result.Project.AssemblyName,compilation,token);
            var compiledRef = compilation.References.OfType<CompilationReference>().ToList();
            foreach (var item in compiledRef)
            {
                Emit(folder, item.Compilation.AssemblyName, item.Compilation, token);
            }
            foreach (var item in compilation.References.OfType<PortableExecutableReference>().Where(x=>x.FilePath.Contains("nuget")).ToList())
            {
                var fn=Path.GetFileName(item.FilePath);
                var path=Path.Combine(folder, fn);
                File.Copy(item.FilePath, path, true);
                //Emit(folder, item.Compilation.AssemblyName, item.Compilation, token);
            }

            return emitResult;
        }
    }
}
