namespace Cli.Params.VariationGenerators
{
    internal abstract class VariationGeneratorParamsBase
    {
        public SuppressorParams[] SuppressorParams { get; set; }
        public string CharMapper { get; set; }
    }
}
