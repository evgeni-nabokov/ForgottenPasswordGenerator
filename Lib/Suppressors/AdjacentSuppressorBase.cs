using System;

namespace Lib.Suppressors
{
    public abstract class AdjacentSuppressorBase : SuppressorBase
    {
        public int MinLength { get; }

        public int MaxLength { get; }

        protected AdjacentSuppressorBase(
            int minLength,
            int maxLength,
            string trackedChars,
            bool ignoreCase = true)
            : base(trackedChars, ignoreCase)
        {
            MaxLength = NormalizeMaxLength(maxLength);
            MinLength = NormalizeMinLength(minLength);
        }

        private int NormalizeMaxLength(int maxLength)
        {
            return Math.Max(LowBoundOfMinLength, maxLength);
        }

        private int NormalizeMinLength(int minLength)
        {
            return Math.Min(Math.Max(LowBoundOfMinLength, minLength), MaxLength);
        }

        protected virtual int LowBoundOfMinLength { get; set; } = 1;
    }
}
