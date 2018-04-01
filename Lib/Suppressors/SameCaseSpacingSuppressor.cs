using System;
using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class SameCaseSpacingSuppressor : SpacingSuppressorBase
    {
        public SameCaseSpacingSuppressor(
            int minSpace = 0,
            int maxSpace = 0,
            string trackedChars = null,
            CharCase trackedCharCase = CharCase.Upper)
            : base(minSpace, maxSpace, trackedChars)
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
            var indices = new List<int>(variation.Length);

            for (var i = 0; i < variation.Length; i++)
            {
                if (IsTrackedChar(variation[i]) && _hasTrackedCase(variation[i]))
                {
                    indices.Add(i);
                }
            }

            if (indices.Count < 2)
            {
                return false;
            }

            for (var i = 1; i < indices.Count; i++)
            {
                var space = indices[i] - indices[i - 1] - 1;
                if (space < MinSpace || space > MaxSpace)
                {
                    return true;
                }
            }

            return false;
        }

        private readonly Func<char, bool> _hasTrackedCase;
    }
}
