using Lib.VariationGenerators;

namespace Cli.Params.VariationGenerators
{
    internal class ArbitraryVariationGeneratorParams : VariationGeneratorParamsBase
    {
        public string Chars { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public CharCase CharCase { get; set; }

}
}
