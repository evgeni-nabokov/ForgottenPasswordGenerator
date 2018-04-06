using System;
using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class AdjacentDuplicatesSuppressor : AdjacentSuppressorBase
    {
        public AdjacentDuplicatesSuppressor(
            int minLength = 1,
            int maxLength = 1,
            string trackedChars = null,
            bool ignoreCase = true)
            : base(minLength, maxLength, trackedChars)
        {
            IgnoreCase = ignoreCase;
        }

        public bool IgnoreCase { get; }

        public override bool BreaksRestrictions(string variation)
        {
            var seqLen = 1;
            for (var i = 1; i < variation.Length; i++)
            {
                var currChar = variation[i];
                var prevChar = variation[i - 1];
                if (IsTrackedChar(currChar) && IsTrackedChar(prevChar))
                {
                    if (CharsAreEqual(currChar, prevChar))
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
                        seqLen = 1;
                    }
                }
                else
                {

                    seqLen = 1;
                }
            }
            return false;
        }

        private bool CharsAreEqual(char c1, char c2)
        {
            return IgnoreCase && char.ToUpper(c1) == char.ToUpper(c2)
                   || !IgnoreCase && c1 == c2;
        }
    }
}
