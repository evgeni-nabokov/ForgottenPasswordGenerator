using Lib.Suppressors;

namespace Cli.Params.Suppressors
{
    internal class AdjacentSameCaseSuppressorParams : SuppressorParamsBase
    {
        public string TrackedChars { get; set; }
        public int MinLength { get; set; }
        public int MaxLength { get; set; }
        public CharCase CharCase { get; set; }
    }
}
