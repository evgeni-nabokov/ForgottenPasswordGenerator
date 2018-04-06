using System;

namespace Lib.Suppressors
{
    public class AdjacentSameCaseSuppressor : SuppressorBase
    {
        public int MinLength => MinInternal;

        public int MaxLength => MaxInternal;

        public CharCase TrackedCharCase => TrackedCharCaseInternal;

        public AdjacentSameCaseSuppressor(
            int minLength = 0,
            int maxLength = 1,
            string trackedChars = null,
            CharCase trackedCharCase = CharCase.Upper)
            : base(minLength, maxLength, trackedChars, trackedCharCase, true)
        {
        }

        public override bool BreaksRestrictions(string variation)
        {
            var seqLen = 0;
            for (var i = 0; i < variation.Length; i++)
            {
                var c = variation[i];
                if (HasTrackedCase(c) && IsTrackedChar(c))
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
    }
}
