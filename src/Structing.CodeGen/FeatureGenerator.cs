using Microsoft.CodeAnalysis;
using Structing.CodeGen.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.CodeGen
{
    [Generator]
    public class FeatureGenerator : IIncrementalGenerator
    {
        private IncrementalGeneratorInitializationContext context;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            this.context = context;
            var syntaxProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(FeatureConsts.Name, FeatureParser.Predicate, FeatureParser.Transform)
                .Where(x => x != null);
            context.RegisterSourceOutput(syntaxProvider, Execute);
        }
        private void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol?>? node)
        {
            var parser = new FeatureParser();
            parser.Execute(context, node!);
        }
    }
}
