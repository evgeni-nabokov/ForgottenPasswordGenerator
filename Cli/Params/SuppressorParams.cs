using Lib.Suppressors;

namespace Cli.Params
{
    internal abstract class SuppressorParams
    {
        internal SuppressorType Type { get; set; }

        public string TrackedChars { get; set; }

        public bool IgnoreCase { get; set; } = true;

        public CharCase CharCase { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public int MinOccurence { get; set; }

        public int MaxOccurence { get; set; } = 1;

        public string Pattern { get; set; }
    }

    internal enum SuppressorType
    {
        AdjacentDuplicates,
        AdjacentSameCase,
        DuplicatesSpacing,
        SameCaseSpacing,
        Regex,
        CharOccurence
    }
}
