using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Structing.CodeGen.Internal
{
    internal class FeatureParser
    {
        private string ValueToCSharp(object? value)
        {
            if (value == null)
            {
                return "null";
            }
            if (value is string str)
            {
                return $"\"{str}\"";
            }
            return value.ToString();
        }
        private readonly HashSet<string> keys = new HashSet<string>();

        public void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol?> node)
        {
            var typeSymbol = (INamedTypeSymbol)node.Value!;
            var attr = typeSymbol.GetAttributes()
                .First(x => x.AttributeClass?.ToString() == FeatureConsts.Name);
            var val = ValueToCSharp(attr.ConstructorArguments[0].Value);
            var aliasType = attr.NamedArguments.FirstOrDefault(x => x.Key == FeatureConsts.Type).Value.Value as INamedTypeSymbol;
            var extenName = attr.NamedArguments.FirstOrDefault(x => x.Key == FeatureConsts.ExtensionName).Value.Value?.ToString()?? "FeatureExtensions";
            if (aliasType != null)
            {
                typeSymbol = aliasType;
            }
            var name = typeSymbol!.Name;
            if (!keys.Add(name))
            {
                return;
            }
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
