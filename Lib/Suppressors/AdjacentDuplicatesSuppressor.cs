namespace Lib.Suppressors
{
    public class AdjacentDuplicatesSuppressor : SuppressorBase
    {
        public int MinLength => MinInternal;

        public int MaxLength => MaxInternal;

        public bool IgnoreCase => IgnoreCaseInternal;

        protected new const int LowBound = 1;

        public AdjacentDuplicatesSuppressor(
            int minLength = 1,
            int maxLength = 1,
            string trackedChars = null,
            bool ignoreCase = true)
            : base(minLength, maxLength, trackedChars, CharCase.Lower, ignoreCase)
        {
        }

        public override bool BreaksRestrictions(string variation)
        {
            var seqLen = 1;
            for (var i = 1; i < variation.Length; i++)
            {
                var currChar = variation[i];
                var prevChar = variation[i - 1];
                if (IsTrackedChar(currChar) && IsTrackedChar(prevChar))
                {
                    if (CharsAreEqual(currChar, prevChar))
                    {
                        seqLen++;
                        if (seqLen > MaxLength)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (seqLen < MinLength)
                        {
                            return true;
                        }
                        seqLen = 1;
                    }
                }
                else
                {

                    seqLen = 1;
                }
            }
            return false;
        }

        private bool CharsAreEqual(char c1, char c2)
        {
            return IgnoreCaseInternal && char.ToUpper(c1) == char.ToUpper(c2)
                   || !IgnoreCaseInternal && c1 == c2;
        }
    }
}
