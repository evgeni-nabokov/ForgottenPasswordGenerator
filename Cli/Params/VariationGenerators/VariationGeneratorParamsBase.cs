using Cli.Params.Suppressors;

namespace Cli.Params.VariationGenerators
{
    internal abstract class VariationGeneratorParamsBase
    {
        public SuppressorParamsBase[] SuppressorParams { get; set; }
        public string CharMapper { get; set; }
    }
}
