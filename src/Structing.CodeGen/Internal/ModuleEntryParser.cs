using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Structing.CodeGen.Internal
{
    internal static class ModuleEntryConst
    {
        public const string ModuleEntryAttribute = "Structing.Annotations.ModuleEntryAttribute";
        public const string ModulePartAttribute = "Structing.Annotations.ModulePartAttribute";
        public const string ModuleIniterAttribute = "Structing.Annotations.ModuleIniterAttribute";
        public const string IRegisteContext = "Structing.IRegisteContext";
        public const string IReadyContext = "Structing.IReadyContext";
        public const string Task = "System.Threading.Tasks.Task";
        public const string Positions = "Positions";
    }
    internal class ModuleEntryParser
    {
        readonly struct MethodInfo
        {
            public readonly IMethodSymbol Method;

            public readonly AttributeData Attribute;

            public readonly int Position;

            public readonly string Call;

            public MethodInfo(IMethodSymbol method,AttributeData attribute)
            {
                Position = 1;
                Method = method;
                Attribute = attribute;
                var posArg = attribute.NamedArguments.FirstOrDefault(x => x.Key == ModuleEntryConst.Positions);
                _ = posArg.Value.Value == null &&int.TryParse(posArg.Value.Value?.ToString(), out Position);
                if (Position < 0 && Position > 3)
                {
                    Position = 1;
                }
                Call = $"{(method.ReturnsVoid?string.Empty:"await ")}{method.ReceiverType}.{method.Name}(context);";
            }
        }

        public void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol?> node)
        {
            var model = node.SyntaxContext.SemanticModel;
            var modulePart = new List<MethodInfo>();
            var moduleInit = new List<MethodInfo>();
            foreach (var item in node.AssemblySymbol.TypeNames)
            {
                var comp = model.Compilation.GetTypeByMetadataName($"{node.AssemblySymbol.Name}.{item}");
                if (comp != null && comp.TypeKind == TypeKind.Class)
                {
                    foreach (var method in comp.GetMembers().OfType<IMethodSymbol>())
                    {
                        if (method.IsStatic &&
                            method.DeclaredAccessibility != Accessibility.Private)
                        {
                            var attributes = method.GetAttributes();
                            if (attributes.Length == 0)
                            {
                                continue;
                            }
                            var attrNames = new HashSet<string>(attributes.Select(x => x.AttributeClass?.ToString()).Where(x => !string.IsNullOrWhiteSpace(x))!);
                            if (attrNames.Contains(ModuleEntryConst.ModulePartAttribute))
                            {
                                if (!IsModulePart(method))
                                {
                                    context.ReportDiagnostic(Diagnostic.Create(Messages.ModulePartDefineFail, null, Array.Empty<string>()));
                                    continue;
                                }
                                modulePart.Add(new MethodInfo(method, attributes.First(x => x.AttributeClass?.ToString() == ModuleEntryConst.ModulePartAttribute)));
                            }
                            else if (attrNames.Contains(ModuleEntryConst.ModuleIniterAttribute))
                            {
                                if (!IsModuleInit(method))
                                {
                                    context.ReportDiagnostic(Diagnostic.Create(Messages.ModuleInitDefineFail, null, Array.Empty<string>()));
                                    continue;
                                }
                                moduleInit.Add(new MethodInfo(method, attributes.First(x => x.AttributeClass?.ToString() == ModuleEntryConst.ModuleIniterAttribute)));
                            }
                        }
                    }
                }
            }
            if (modulePart.Count == 0 && moduleInit.Count == 0)
            {
                return;
            }
            node.GetWriteNameSpace(out var nsStart, out var nsEnd);
            var name = node.Value!.Name;
            var code = $@"
{nsStart}

    partial class {name}
    {{        
        public sealed override void ReadyRegister(global::Structing.IRegisteContext context)
        {{
            {string.Join("\n", modulePart.Where(x => x.Position == 0).Select(x => x.Call))}
            OnReadyRegister(context);
            base.ReadyRegister(context);
        }}
        partial void OnReadyRegister(global::Structing.IRegisteContext context);

        public sealed override void Register(global::Structing.IRegisteContext context)
        {{
            {string.Join("\n", modulePart.Where(x => x.Position == 1).Select(x => x.Call))}
            OnRegister(context);
            base.Register(context);
        }}

        partial void OnRegister(global::Structing.IRegisteContext context);

        public sealed override void AfterRegister(global::Structing.IRegisteContext context)
        {{
            {string.Join("\n", modulePart.Where(x => x.Position == 2).Select(x => x.Call))}
            OnAfterRegister(context);
            base.AfterRegister(context);
        }}
        partial void OnAfterRegister(global::Structing.IRegisteContext context);

        public override async global::System.Threading.Tasks.Task BeforeReadyAsync(global::Structing.IReadyContext context)
        {{
            {string.Join("\n", moduleInit.Where(x => x.Position == 0).Select(x => x.Call))}
            await base.BeforeReadyAsync(context);
        }}
        public override async global::System.Threading.Tasks.Task ReadyAsync(global::Structing.IReadyContext context)
        {{
            {string.Join("\n", moduleInit.Where(x => x.Position == 1).Select(x => x.Call))}
            await base.ReadyAsync(context);
        }}
        public override async global::System.Threading.Tasks.Task AfterReadyAsync(global::Structing.IReadyContext context)
        {{
            {string.Join("\n", moduleInit.Where(x => x.Position == 2).Select(x => x.Call))}
            await base.AfterReadyAsync(context);
        }}    
    }}

{nsEnd}
";
            code = Helpers.FormatCode(code);
            context.AddSource($"{name}.g.cs", code);

        }
        private static bool IsModulePart(IMethodSymbol symbol)
        {
            return symbol.ReturnsVoid && 
                symbol.Parameters.Length == 1 &&
                symbol.Parameters[0].Type.ToString() == ModuleEntryConst.IRegisteContext;
        }
        private static bool IsModuleInit(IMethodSymbol symbol)
        {
            return symbol.ReturnType?.ToString()== ModuleEntryConst.Task && 
                symbol.Parameters.Length == 1 &&
                symbol.Parameters[0].Type.ToString() == ModuleEntryConst.IReadyContext;
        }
        public static GeneratorTransformResult<ISymbol?>? Transform(GeneratorAttributeSyntaxContext context, CancellationToken token)
        {
            return new GeneratorTransformResult<ISymbol?>(context.TargetSymbol, context);
        }
        public static bool Predicate(SyntaxNode node, CancellationToken token)
        {
            return true;
        }
    }
}
