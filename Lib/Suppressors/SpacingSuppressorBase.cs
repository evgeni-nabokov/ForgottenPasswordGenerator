using System;
using System.Collections.Generic;

namespace Lib.Suppressors
{
    public abstract class SpacingSuppressorBase : ISuppressor
    {
        public int MinSpace { get; }

        public int MaxSpace { get; }

        public string TrackedChars { get; }

        protected SpacingSuppressorBase(int minSpace, int maxSpace, string trackedChars)
        {
            MaxSpace = NormalizeMaxSpace(maxSpace);
            MinSpace = NormalizeMinSpace(minSpace);
            TrackedChars = trackedChars;
            IsEmpty = string.IsNullOrEmpty(trackedChars);
            if (!IsEmpty)
            {
                TrackedCharset = new HashSet<char>(trackedChars ?? string.Empty);
            }
        }

        public abstract bool BreaksRestrictions(string variation);

        protected bool IsTrackedChar(char c)
        {
            return IsEmpty || TrackedCharset.Contains(c);
        }

        private int NormalizeMaxSpace(int maxSpace)
        {
            return Math.Max(0, maxSpace);
        }

        private int NormalizeMinSpace(int minSpace)
        {
            return Math.Min(Math.Max(0, minSpace), MaxSpace);
        }

        protected bool IsEmpty;

        protected HashSet<char> TrackedCharset;
    }
}
