using System;
using System.Collections.Generic;

namespace Lib.Suppressors
{
    public abstract class SuppressorBase : ISuppressor
    {
        public string TrackedChars { get; }

        public abstract bool BreaksRestrictions(string variation);

        protected bool IgnoreCaseInternal;

        protected CharCase TrackedCharCaseInternal;

        protected bool IsEmpty;

        protected int MaxInternal;

        protected int MinInternal;

        protected HashSet<char> TrackedCharset;

        protected const int LowBound = 0;

        protected readonly Func<char, bool> HasTrackedCase;

        protected SuppressorBase(int min, int max, string trackedChars, CharCase trackedCharCase, bool ignoreCase)
        {
            MaxInternal = NormalizeMax(max);
            MinInternal = NormalizeMin(min, max);
            TrackedChars = trackedChars;
            TrackedCharCaseInternal = trackedCharCase;
            if (trackedCharCase == CharCase.Lower)
            {
                HasTrackedCase = char.IsLower;
            }
            else
            {
                HasTrackedCase = char.IsUpper;
            }
            IgnoreCaseInternal = ignoreCase;
            BuildTrackedCharset();
            IsEmpty = TrackedCharset == null || TrackedCharset.Count > 0;
        }

        private void BuildTrackedCharset()
        {
            if (!string.IsNullOrEmpty(TrackedChars))
            {
                TrackedCharset = new HashSet<char>();

                for (var i = 0; i < TrackedChars.Length; i++)
                {
                    var c = TrackedChars[i];
                    if (char.IsLetter(c) && IgnoreCaseInternal)
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

        protected bool IsTrackedChar(char c)
        {
            return IsEmpty || TrackedCharset.Contains(c);
        }

        private static int NormalizeMax(int max)
        {
            return Math.Max(LowBound, max);
        }

        private static int NormalizeMin(int minOccurence, int maxOccurence)
        {
            return Math.Min(Math.Max(LowBound, minOccurence), maxOccurence);
        }
    }
}
