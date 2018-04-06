using System.Collections.Generic;

namespace Lib.Suppressors
{
    public class SameCaseSpacingSuppressor : SuppressorBase
    {
        public int MinSpace => MinInternal;

        public int? MaxSpace => MaxInternal;

        public CharCase TrackedCharCase => TrackedCharCaseInternal;

        public SameCaseSpacingSuppressor(
            int minSpace = 0,
            int? maxSpace = 1,
            string trackedChars = null,
            CharCase trackedCharCase = CharCase.Upper)
            : base(minSpace, maxSpace, trackedChars, trackedCharCase, true)
        {
        }

        public override bool BreaksRestrictions(string variation)
        {
            var indices = new List<int>(variation.Length);

            for (var i = 0; i < variation.Length; i++)
            {
                if (IsTrackedChar(variation[i]) && HasTrackedCase(variation[i]))
                {
                    indices.Add(i);
                }
            }

            if (indices.Count < 2)
            {
                return false;
            }

            for (var i = 1; i < indices.Count; i++)
            {
                var space = indices[i] - indices[i - 1] - 1;
                if (space < MinSpace || MaxSpace.HasValue && space > MaxSpace.Value)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
