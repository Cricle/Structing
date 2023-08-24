using Microsoft.CodeAnalysis;
using Structing.CodeGen.Internal;

namespace Structing.CodeGen
{
    [Generator]
    public class ModuleEntryGenerator : IIncrementalGenerator
    {
        private IncrementalGeneratorInitializationContext context;

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            this.context = context;
            var syntaxProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(ModuleEntryConst.ModuleEntryAttribute, ModuleEntryParser.Predicate, ModuleEntryParser.Transform)
                .Where(x => x != null);
            context.RegisterSourceOutput(syntaxProvider, Execute);
        }
        private void Execute(SourceProductionContext context, GeneratorTransformResult<ISymbol?>? node)
        {
            var parser = new ModuleEntryParser();
            parser.Execute(context, node!);
        }
    }
}
