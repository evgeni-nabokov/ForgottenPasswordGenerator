using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class DuplicatesSpacingSuppressor : SpacingSuppressorBase
    {
        public DuplicatesSpacingSuppressor(
            int minSpace = 1,
            int maxSpace = 1,
            string trackedChars = null,
            bool ignoreCase = true)
            : base(minSpace, maxSpace, trackedChars)
        {
            IgnoreCase = ignoreCase;
            if (!string.IsNullOrEmpty(TrackedChars))
            {
                BuildTrackedCharset();
                IsEmpty = false;
            }
            else
            {
                IsEmpty = true;
            }
        }

        public bool IgnoreCase { get; }

        public override bool BreaksRestrictions(string variation)
        {
            var foundTrackedChars = new Dictionary<char, IList<int>>();

            for (var i = 0; i < variation.Length; i++)
            {
                var currChar = variation[i];
                if (IsTrackedChar(currChar))
                {
                    if (IgnoreCase)
                    {
                        currChar = char.ToUpper(currChar);
                    }
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
                    if (space < MinSpace || space > MaxSpace)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void BuildTrackedCharset()
        {
            TrackedCharset = new HashSet<char>(TrackedChars.Length);

            for (var i = 0; i < TrackedChars.Length; i++)
            {
                var c = TrackedChars[i];
                if (char.IsLetter(c) && IgnoreCase)
                {
                    TrackedCharset.Add(char.ToLower(c));
                    TrackedCharset.Add(char.ToUpper(c));
                }
                else
                {
                    TrackedCharset.Add(c);
                }
            }
        }
    }
}
