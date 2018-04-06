﻿using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class CharOccurenceSuppressor : SuppressorBase
    {
        public int MinOccurence => MinInternal;

        public int MaxOccurence => MaxInternal;

        public CharOccurenceSuppressor(
            int minOccurence = 0,
            int maxOccurence = 1,
            string trackedChars = null,
            bool ignoreCase = true)
            : base(minOccurence, maxOccurence, trackedChars, CharCase.Lower, ignoreCase)
        {
            _counter = new Dictionary<char, int>();
        }

        public override bool BreaksRestrictions(string variation)
        {
            for (var i = 0; i < variation.Length; i++)
            {
                var currChar = variation[i];
                if (IsTrackedChar(currChar))
                {
                    currChar = IgnoreCaseInternal ? char.ToUpper(currChar) : currChar;
                    _counter.TryGetValue(currChar, out var currentCount);
                    _counter[currChar] = currentCount + 1;
                    if (_counter[currChar] > MaxOccurence)
                    {
                        _counter.Clear();
                        return true;
                    }
                }
            }
            _counter.Clear();
            return false;
        }

        private readonly IDictionary<char, int> _counter;
    }
}
