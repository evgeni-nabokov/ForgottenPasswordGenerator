using Lib.VariationGenerators;

namespace Cli.Params.VariationGenerators
{
    internal class FixedVariationGeneratorParams : VariationGeneratorParamsBase
    {
        public string Chars { get; set; }
        public int? MinLength { get; set; }
        public CharCase CharCase { get; set; } = CharCase.AsDefined;
    }
}
