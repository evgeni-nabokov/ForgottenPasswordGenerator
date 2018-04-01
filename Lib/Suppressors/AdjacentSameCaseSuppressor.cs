using System;
using System.Collections.Generic;

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

            if (!string.IsNullOrEmpty(TrackedChars))
            {
                BuildTrackedCharset();
                IsEmpty = false;
            }
            else
            {
                IsEmpty = true;
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

        private void BuildTrackedCharset()
        {
            TrackedCharset = new HashSet<char>(TrackedChars.Length);

            for (var i = 0; i < TrackedChars.Length; i++)
            {
                var c = TrackedChars[i];
                if (char.IsLetter(c))
                {
                    c = TrackedCharCase == CharCase.Lower ? Char.ToLower(c) : Char.ToUpper(c);
                }
                TrackedCharset.Add(c);
            }
        }

        private readonly Func<char, bool> _hasTrackedCase;

        protected override int LowBoundOfMinLength { get; set; } = 0;
    }
}
