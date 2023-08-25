using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Structing.CodeGen
{
    internal static class Consts
    {
        public const string Generate = "[global::System.CodeDom.Compiler.GeneratedCode(\"Structing.CodeGen\",\"1.0.0\")]";
        public const string CompilerGenerated = "[global::System.Runtime.CompilerServices.CompilerGenerated]";
        public const string DebuggerStepThrough = "[global::System.Diagnostics.DebuggerStepThrough]";
    }
    internal static class Helpers
    {
        public static string FormatCode(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetRoot();
            return root.NormalizeWhitespace().ToFullString();
        }
    }
}
