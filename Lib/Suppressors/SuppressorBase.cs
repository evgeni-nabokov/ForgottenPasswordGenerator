using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.Suppressors
{
    public abstract class SuppressorBase : ISuppressor
    {
        public string TrackedChars { get; }

        public abstract bool BreaksRestrictions(string variation);

        protected SuppressorBase(string trackedChars, bool ignoreCase = true)
        {
            TrackedChars = trackedChars;
            if (!string.IsNullOrEmpty(trackedChars))
            {
                TrackedCharset = new HashSet<char>();

                for (var i = 0; i < TrackedChars.Length; i++)
                {
                    var c = TrackedChars[i];
                    if (char.IsLetter(c) && ignoreCase)
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
            IsEmpty = TrackedCharset == null || TrackedCharset.Count > 0;
        }

        private void BuildTrackedCharset()
        {

        }

        protected bool IsTrackedChar(char c)
        {
            return IsEmpty || TrackedCharset.Contains(c);
        }

        protected bool IsEmpty;

        protected HashSet<char> TrackedCharset;
    }
}
