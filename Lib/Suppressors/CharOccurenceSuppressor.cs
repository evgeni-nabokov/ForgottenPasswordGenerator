using System;
using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class CharOccurenceSuppressor : SuppressorBase
    {
        public CharOccurenceSuppressor(
            int minOccurence = 0,
            int maxOccurence = 1,
            string trackedChars = null,
            bool ignoreCase = true)
            : base(trackedChars, ignoreCase)
        {
            MaxOccurence = NormalizeMaxOccurence(maxOccurence);
            MinOccurence = NormalizeMinOccurence(minOccurence, MaxOccurence);
            _counter = new Dictionary<char, int>();
        }

        public int MinOccurence { get; }

        public int MaxOccurence { get; }

        public override bool BreaksRestrictions(string variation)
        {
            for (var i = 0; i < variation.Length; i++)
            {
                var c = variation[i];
                if (IsTrackedChar(c))
                {
                    if (MaxOccurence == 0)
                    {
                        return true;
                    }

                    _counter.TryGetValue(c, out var currentCount);
                    _counter[c] = currentCount + 1;
                    if (_counter[c] > MaxOccurence)
                    {
                        _counter.Clear();
                        return true;
                    }
                }
            }
            _counter.Clear();
            return false;
        }

        private static int NormalizeMaxOccurence(int maxOccurence)
        {
            return Math.Max(1, maxOccurence);
        }

        private static int NormalizeMinOccurence(int minOccurence, int maxOccurence)
        {
            return Math.Min(Math.Max(0, minOccurence), maxOccurence);
        }

        private readonly IDictionary<char, int> _counter;
    }
}
