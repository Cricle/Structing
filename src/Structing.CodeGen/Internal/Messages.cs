using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.CodeGen.Internal
{
    internal static class Messages
    {
        public static readonly DiagnosticDescriptor ModulePartDefineFail = new DiagnosticDescriptor(
            "STRUCTING_001",
            "Module part define fail",
            "The model part method must be ** void Any(IRegisteContext context) **, it will not to generate code",
            "STRUCTING",
            DiagnosticSeverity.Warning,
            true
            );
    }
}
