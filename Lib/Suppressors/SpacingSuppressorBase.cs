using System;

namespace Lib.Suppressors
{
    public abstract class SpacingSuppressorBase : SuppressorBase
    {
        public int MinSpace { get; }

        public int MaxSpace { get; }

        protected SpacingSuppressorBase(
            int minSpace,
            int maxSpace,
            string trackedChars,
            bool ignoreCase = true)
            : base(trackedChars, ignoreCase)
        {
            MaxSpace = NormalizeMaxSpace(maxSpace);
            MinSpace = NormalizeMinSpace(minSpace, MaxSpace);
        }

        private static int NormalizeMaxSpace(int maxSpace)
        {
            return Math.Max(0, maxSpace);
        }

        private static int NormalizeMinSpace(int minSpace, int maxSpace)
        {
            return Math.Min(Math.Max(0, minSpace), maxSpace);
        }
    }
}
