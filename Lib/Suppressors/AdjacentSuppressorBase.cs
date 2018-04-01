using System;
using System.Collections.Generic;

namespace Lib.Suppressors
{
    public abstract class AdjacentSuppressorBase : ISuppressor
    {
        public int MinLength { get; }

        public int MaxLength { get; }

        public string TrackedChars { get; }

        protected const int SetSize = 100;

        protected AdjacentSuppressorBase(int minLength, int maxLength, string trackedChars)
        {
            MaxLength = NormalizeMaxLength(maxLength);
            MinLength = NormalizeMinLength(minLength);
            TrackedChars = trackedChars;
        }

        public abstract bool BreaksRestrictions(string variation);

        protected bool IsTrackedChar(char c)
        {
            return IsEmpty || TrackedCharset.Contains(c);
        }

        private int NormalizeMaxLength(int maxLength)
        {
            return Math.Max(LowBoundOfMinLength, maxLength);
        }

        private int NormalizeMinLength(int minLength)
        {
            return Math.Min(Math.Max(LowBoundOfMinLength, minLength), MaxLength);
        }

        protected bool IsEmpty;

        protected HashSet<char> TrackedCharset;

        protected virtual int LowBoundOfMinLength { get; set; } = 1;
    }
}
