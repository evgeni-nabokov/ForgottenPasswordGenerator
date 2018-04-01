namespace Cli.Params.VariationGenerators
{
    internal class CompoundVariationGeneratorParams : VariationGeneratorParamsBase
    {
        public VariationGeneratorParamsBase[] Generators { get; set; }
    }
}
