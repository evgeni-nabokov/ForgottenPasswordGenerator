using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class DuplicatesSpacingSuppressor : SuppressorBase
    {
        public int MinSpace => MinInternal;

        public int? MaxSpace => MaxInternal;

        public bool IgnoreCase => IgnoreCaseInternal;

        public DuplicatesSpacingSuppressor(
            int minSpace = 0,
            int? maxSpace = 1,
            string trackedChars = null,
            bool ignoreCase = true)
            : base(minSpace, maxSpace, trackedChars, CharCase.Lower, ignoreCase)
        {
        }

        public override bool BreaksRestrictions(string variation)
        {
            if (MinSpace == 0 && !MaxSpace.HasValue)
            {
                return false;
            }

            var foundTrackedChars = new Dictionary<char, IList<int>>();

            for (var i = 0; i < variation.Length; i++)
            {
                var currChar = variation[i];
                if (IsTrackedChar(currChar))
                {
                    currChar = IgnoreCaseInternal ? char.ToUpper(currChar) : currChar;
                    if (!foundTrackedChars.TryGetValue(currChar, out var indexList))
                    {
                        indexList = new List<int>();
                        foundTrackedChars[currChar] = indexList;
                    }
                    indexList.Add(i);
                }
            }

            foreach (var indexList in foundTrackedChars.Values)
            {
                for (var i = 1; i < indexList.Count; i++)
                {
                    var space = indexList[i] - indexList[i - 1] - 1;
                    if (space < MinSpace || MaxSpace.HasValue && space > MaxSpace.Value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
