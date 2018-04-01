using System.Text.RegularExpressions;

namespace Lib.Suppressors
{
    public class RegexSuppressor : ISuppressor
    {
        public RegexSuppressor(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled);
        }

        public bool BreaksRestrictions(string variation)
        {
            return _regex.IsMatch(variation);
        }

        private readonly Regex _regex;
    }
}
