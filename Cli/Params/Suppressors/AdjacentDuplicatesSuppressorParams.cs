namespace Cli.Params.Suppressors
{
    internal class AdjacentDuplicatesSuppressorParams : SuppressorParamsBase
    {
        public string TrackedChars { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
    }
}
