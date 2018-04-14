using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class CharOccurenceSuppressor : SuppressorBase
    {
        public int MinOccurs => MinInternal;

        public int? MaxOccurs => MaxInternal;

        public CharOccurenceSuppressor(
            int minOccurs = 0,
            int? maxOccurs = 1,
            string trackedChars = null,
            bool ignoreCase = true)
            : base(minOccurs, maxOccurs, trackedChars, CharCase.Lower, ignoreCase)
        {
            _counter = new Dictionary<char, int>();
        }

        public override bool BreaksRestrictions(string variation)
        {
            if (MinOccurs == 0 && !MaxOccurs.HasValue)
            {
                return false;
            }

            for (var i = 0; i < variation.Length; i++)
            {
                var currChar = variation[i];
                if (IsTrackedChar(currChar))
                {
                    currChar = IgnoreCaseInternal ? char.ToUpper(currChar) : currChar;
                    _counter.TryGetValue(currChar, out var currentCount);
                    currentCount++;
                    if (MaxOccurs.HasValue && currentCount > MaxOccurs.Value)
                    {
                        _counter.Clear();
                        return true;
                    }
                    _counter[currChar] = currentCount;
                }
            }

            if (MinOccurs > 0)
            {
                foreach (var occurences in _counter.Values)
                {
                    if (occurences < MinOccurs)
                    {
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
