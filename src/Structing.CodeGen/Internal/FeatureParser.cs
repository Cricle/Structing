using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Structing.CodeGen.Internal
{
    internal class FeatureParser
    {
        private readonly Dictionary<ITypeSymbol, Dictionary<decimal, string>> enumMap = new Dictionary<ITypeSymbol, Dictionary<decimal, string>>(SymbolEqualityComparer.Default);

        private string? ParseEnum(object value, ITypeSymbol typeSymbol)
        {
            Debug.Assert(typeSymbol.TypeKind == TypeKind.Enum);
            var left = (decimal)Convert.ChangeType(value, typeof(decimal));
            if (!enumMap.TryGetValue(typeSymbol, out var map))
            {
                map = typeSymbol.GetMembers().OfType<IFieldSymbol>().ToDictionary(x => (decimal)Convert.ChangeType(x.ConstantValue, typeof(decimal)), x => x.Name);
                enumMap[typeSymbol] = map;
            }
            return map.TryGetValue(left, out var str) ? str : null;
        }

        private string ValueToCSharp(object? value,ITypeSymbol? typeSymbol)
        {
            if (value == null)
                return "null";
            if (typeSymbol!=null&&typeSymbol.TypeKind== TypeKind.Enum)
            {
                var res= ParseEnum(value, typeSymbol);
                if (string.IsNullOrEmpty(res))
                {
                    return $"(global::{typeSymbol}){value}";
                }
                return $"global::{typeSymbol}.{res}";
            }
            if (value is string str)
                return $"\"{str}\"";
            if (value is bool b)
                return b ? "true" : "false";
            if (value is ulong ul)
                return $"{ul}UL";
            if (value is double d)
                return $"{d}d";
            if (value is float f)
                return $"{f}f";
            if (value is uint ui)
                return $"{ui}u";
            if (value is byte @byte)
                return $"(byte){@byte}";
            if (value is sbyte sb)
                return $"(sbyte){sb}";
            if (value is char c)
                return $"(char){c}";
            if (value is short s)
                return $"(short){s}";
            if (value is ushort us)
                return $"(ushort){us}";
            return value.ToString();
        }
        public void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol?> node)
        {
            var targetType = node.SyntaxContext.TargetSymbol;
            if (node.Value != null)
            {
                var processingedEmpty = false;
                var processedType = new HashSet<string>();
                var attributes = targetType.GetAttributes().Where(x => x.AttributeClass?.ToString() == FeatureConsts.Name).ToList();
                foreach (var attr in attributes)
                {
                    var attrType = (attr.NamedArguments.FirstOrDefault(x => x.Key == FeatureConsts.Type).Value.Value as INamedTypeSymbol)?.OriginalDefinition;
                    targetType = attrType ?? targetType;
                    if (targetType.IsStatic)
                    {
                        continue;//NOTE: Now is not support static class
                    }
                    var newNode = new GeneratorTransformResult<ISymbol>(targetType, node.SyntaxContext);
                    if (attrType != null && processedType.Add(attrType.ToString()))
                    {
                        ExecuteOne(context, newNode, attr, attrType);
                    }
                    else if (!processingedEmpty)
                    {
                        processingedEmpty = true;
                        ExecuteOne(context, newNode, attr, targetType!);
                    }
                }
            }
        }
        private void ExecuteOne(SourceProductionContext context, GeneratorTransformResult<ISymbol> node, AttributeData data, ISymbol targetType)
        {
            var typeSymbol = (INamedTypeSymbol)node.Value!;
            var constFirst = data.ConstructorArguments[0];
            var val = ValueToCSharp(constFirst.Value, constFirst.Type);
            var extenName = data.NamedArguments.FirstOrDefault(x => x.Key == FeatureConsts.ExtensionName).Value.Value?.ToString()?? "FeatureExtensions";
            var name = typeSymbol!.Name;
            var visibility = node.GetAccessibilityString();
            var nameSpace = "namespace "+node.GetNameSpace()+"\n{";
            var endNameSpace = "}";
            if (nameSpace.Contains("<global namespace>"))
            {
                endNameSpace = string.Empty;
                nameSpace = string.Empty;
            }
            var type = "global::" + typeSymbol.ToString();
            var code = $@"
{nameSpace}
    {Consts.Generate}
    {Consts.CompilerGenerated}
    [global::Structing.Core.Annotations.FeatureRegisterAttribute(Key = {val}, Type = typeof({type}))]
    {visibility} static class {name}{extenName}
    {{        
        public static readonly System.Object Key = {val};

        public static System.Boolean Has{name}(this {Interfaces.IFeatureContext} ctx)
        {{
            return ctx.Features.Contains({val});
        }}
        public static System.Boolean TrySet{name}(this {Interfaces.IFeatureContext} ctx, {type} value)
        {{
            if(!ctx.Features.Contains({val}))
            {{
                ctx.Features[{val}] = value;
                return true;
            }}
            return false;
        }}
        public static void Set{name}(this {Interfaces.IFeatureContext} ctx, {type} value)
        {{
            ctx.Features[{val}] = value;
        }}
        public static void Remove{name}(this {Interfaces.IFeatureContext} ctx)
        {{
            ctx.Features.Remove({val});
        }}
        public static System.Boolean TryGet{name}(this {Interfaces.IFeatureContext} ctx,out {type} value)
        {{
            if(ctx.Features.Contains({val}))
            {{
                value = ({type})ctx.Features[{val}];
                return true;
            }}
            value=default({type});
            return false;
        }}
        public static {type} Get{name}(this {Interfaces.IFeatureContext} ctx)
        {{
            TryGet{name}(ctx,out var value);
            return value;
        }}
    }}
{endNameSpace}
";
            context.AddSource($"{name}Feature.g.cs", Helpers.FormatCode(code));
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
