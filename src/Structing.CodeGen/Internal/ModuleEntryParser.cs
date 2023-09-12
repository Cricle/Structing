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
        public const string ModuleIgnoreAttribute = "Structing.Annotations.ModuleIgnoreAttribute";
        public const string IRegisteContext = "Structing.IRegisteContext";
        public const string AutoModuleEntry = "Structing.AutoModuleEntry";
        public const string IReadyContext = "Structing.IReadyContext";
        public const string Task = "System.Threading.Tasks.Task";
        public const string Positions = "Positions";
        public const string Order = "Order";
    }
    internal class ModuleEntryParser
    {
        enum ModuleType
        {
            Regist,
            Init
        }
        readonly struct MethodInfo
        {
            public readonly IMethodSymbol Method;

            public readonly AttributeData Attribute;

            public readonly int Position;

            public readonly string Call;

            public readonly int Order;

            public readonly IReadOnlyList<string>? Paramters;

            public readonly ModuleType ModuleType;

            public MethodInfo(IMethodSymbol method, AttributeData attribute, ModuleType moduleType)
            {
                ModuleType = moduleType;
                Position = 1;
                Method = method;
                Attribute = attribute;
                var posArg = attribute.NamedArguments.FirstOrDefault(x => x.Key == ModuleEntryConst.Positions);
                var orderArg = attribute.NamedArguments.FirstOrDefault(x => x.Key == ModuleEntryConst.Order);
                _ = posArg.Value.Value == null && int.TryParse(posArg.Value.Value?.ToString(), out Position);
                if (Position < 0 && Position > 3)
                {
                    Position = 1;
                }
                _ = orderArg.Value.Value != null && int.TryParse(orderArg.Value.Value?.ToString(), out Order);
                if (ModuleType == ModuleType.Init)
                {
                    var pars = new List<string>();
                    foreach (var item in method.Parameters)
                    {
                        if (item.Type?.ToString() == ModuleEntryConst.IReadyContext)
                        {
                            pars.Add("context");
                        }
                        else
                        {
                            pars.Add($"context.GetRequiredService<global::{item.Type}>()");
                        }
                    }
                    Paramters = pars;
                    Call = $"{(method.ReturnsVoid ? string.Empty : "await ")}global::{method.ReceiverType}.{method.Name}({string.Join(", ", pars)});";
                }
                else
                {
                    Call = $"{(method.ReturnsVoid ? string.Empty : "await ")}global::{method.ReceiverType}.{method.Name}(context);";
                }
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
                    var attrs = comp.GetAttributes();
                    var partAttrData = attrs.FirstOrDefault(x => x.AttributeClass?.ToString() == ModuleEntryConst.ModulePartAttribute);
                    var initAttrData = attrs.FirstOrDefault(x => x.AttributeClass?.ToString() == ModuleEntryConst.ModuleIniterAttribute);
                    foreach (var method in comp.GetMembers().OfType<IMethodSymbol>())
                    {
                        if (method.IsStatic &&
                            method.DeclaredAccessibility != Accessibility.Private)
                        {
                            var attributes = method.GetAttributes();
                            if ((attributes.Length == 0&&partAttrData==null&&initAttrData==null) ||
                                attributes.Any(x => x.AttributeClass?.ToString() == ModuleEntryConst.ModuleIgnoreAttribute))
                            {
                                continue;
                            }
                            var attrNames = new HashSet<string>(attributes.Select(x => x.AttributeClass?.ToString()).Where(x => !string.IsNullOrWhiteSpace(x))!);
                            if (attrNames.Contains(ModuleEntryConst.ModulePartAttribute)|| partAttrData!=null)
                            {
                                if (!IsModulePart(method))
                                {
                                    context.ReportDiagnostic(Diagnostic.Create(Messages.ModulePartDefineFail, method.Locations[0], Array.Empty<string>()));
                                    if (partAttrData==null)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    modulePart.Add(new MethodInfo(method, partAttrData ?? attributes.First(x => x.AttributeClass?.ToString() == ModuleEntryConst.ModulePartAttribute), ModuleType.Regist));
                                }
                            }
                            if (attrNames.Contains(ModuleEntryConst.ModuleIniterAttribute)||initAttrData!=null)
                            {
                                moduleInit.Add(new MethodInfo(method, initAttrData ?? attributes.First(x => x.AttributeClass?.ToString() == ModuleEntryConst.ModuleIniterAttribute), ModuleType.Init));
                            }
                        }
                    }
                }
            }

            var assemblySymbol = node.Value as IAssemblySymbol;
            if (assemblySymbol==null&&(modulePart.Count == 0 && moduleInit.Count == 0))
            {
                return;
            }
            modulePart.Sort((x, y) => y.Order - x.Order);
            moduleInit.Sort((x, y) => y.Order - x.Order);
            string nsStart;
            string nsEnd;
            string visibility;
            var baseClass = string.Empty;
            var name = node.Value?.Name;
            if (assemblySymbol!=null)
            {
                visibility = "public";
                name = assemblySymbol.Name.Split('.').Last()+"ModuleEntry";
                nsStart =$"namespace {assemblySymbol.Name}\n{{";
                nsEnd = "}";
                baseClass = $": global::{ModuleEntryConst.AutoModuleEntry}";
            }
            else
            {
                visibility = node.GetAccessibilityString();
                node.GetWriteNameSpace(out nsStart, out nsEnd);
            }
            var code = $@"
{nsStart}
    using Microsoft.Extensions.DependencyInjection;
    {Consts.Generate}
    {Consts.CompilerGenerated}
    {Consts.DebuggerStepThrough}
    {visibility} partial class {name} {baseClass}
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
