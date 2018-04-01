namespace Cli.Params.VariationGenerators
{
    internal class NumberRangeVariationGeneratorParams : VariationGeneratorParamsBase
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public int Step { get; set; } = 1;
        public string Format { get; set; }
    }
}
