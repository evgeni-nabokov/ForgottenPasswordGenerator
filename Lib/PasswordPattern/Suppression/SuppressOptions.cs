namespace Lib.PasswordPattern.Suppression
{
    public class SuppressOptions
    {
        internal SuppressOptions() { }

        public SuppressOptions(
            string forbiddenDuplicateChars = null,
            int? adjacentDuplicateMaxLength = null,
            int? capitalAdjacentMaxLength = null,
            int? capitalCharMinDistance = null,
            string regex = null
        )
        {
            ForbiddenDuplicateChars = forbiddenDuplicateChars;
            AdjacentDuplicateMaxLength = adjacentDuplicateMaxLength < 1 ? null : adjacentDuplicateMaxLength;
            CapitalAdjacentMaxLength = capitalAdjacentMaxLength < 1 ? null : capitalAdjacentMaxLength;
            CapitalCharMinDistance = capitalCharMinDistance;
            Regex = regex;
        }

        public string ForbiddenDuplicateChars { get; internal set; }
        public int? AdjacentDuplicateMaxLength { get; internal set; }
        public int? CapitalAdjacentMaxLength { get; internal set; }
        public int? CapitalCharMinDistance { get; internal set; }
        public string Regex { get; set; }
    }
}
