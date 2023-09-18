using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Emit;
using System;

namespace Structing.HotReload.Exceptions
{
    public class HotCompileException : Exception
    {
        public HotCompileException(IProjectCompiledResult compiledResult, EmitResult emitResult, Compilation compilation, string emitFolder)
        {
            CompiledResult = compiledResult;
            EmitResult = emitResult;
            Compilation = compilation;
            EmitFolder = emitFolder;
        }

        public IProjectCompiledResult CompiledResult { get; }

        public EmitResult EmitResult { get; }

        public Compilation Compilation { get; }

        public string EmitFolder { get; }
    }
}
