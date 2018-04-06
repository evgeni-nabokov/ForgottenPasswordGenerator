using System;

namespace Lib.Suppressors
{
    public class AdjacentSameCaseSuppressor : AdjacentSuppressorBase
    {
        public AdjacentSameCaseSuppressor(
            int minLength = 0,
            int maxLength = 1,
            string trackedChars = null,
            CharCase trackedCharCase = CharCase.Upper)
            : base(minLength, maxLength, trackedChars)
        {
            TrackedCharCase = trackedCharCase;
            if (TrackedCharCase == CharCase.Lower)
            {
                _hasTrackedCase = char.IsLower;
            }
            else
            {
                _hasTrackedCase = char.IsUpper;
            }
        }

        public CharCase TrackedCharCase { get; }

        public override bool BreaksRestrictions(string variation)
        {
            var seqLen = 0;
            for (var i = 0; i < variation.Length; i++)
            {
                var c = variation[i];
                if (_hasTrackedCase(c) && IsTrackedChar(c))
                {
                    seqLen++;
                    if (seqLen > MaxLength)
                    {
                        return true;
                    }
                }
                else
                {
                    if (seqLen < MinLength)
                    {
                        return true;
                    }
                    seqLen = 0;
                }
            }
            return false;
        }

        private readonly Func<char, bool> _hasTrackedCase;

        protected override int LowBoundOfMinLength { get; set; } = 0;
    }
}
