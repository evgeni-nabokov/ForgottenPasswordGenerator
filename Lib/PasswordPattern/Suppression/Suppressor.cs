using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Lib.PasswordPattern.Suppression
{
    public class Suppressor : ISuppressor
    {
        public Suppressor(SuppressOptions options)
        {
            Options = options;

            if (options.AdjacentDuplicateMaxLength.HasValue)
            {
                _checkers.Add(BreaksMaxSingeCharSequence);
            }

            if (options.CapitalAdjacentMaxLength.HasValue)
            {
                _checkers.Add(BreaksMaxCapitalLetterSequenceLength);
            }

            if (options.CapitalCharMinDistance.HasValue)
            {
                _checkers.Add(BreaksMinCapitalLetterDistance);
            }

            if (!string.IsNullOrEmpty(options.Regex))
            {
                _regex = new Regex(options.Regex, RegexOptions.Compiled);
                _checkers.Add(BreaksByRegex);
            }

            if (_checkers.Count > 0)
            {
                _breaksRestrictions = BreaksAnyRestriction;
            }
        }

        public readonly SuppressOptions Options;

        public bool BreaksRestrictions(StringBuilder variation)
        {
            return _breaksRestrictions(variation);
        }

        private bool BreaksAnyRestriction(StringBuilder variation)
        {
            for (var i = 0; i < _checkers.Count; i++)
            {
                if (_checkers[i](variation))
                {
                    return true;
                }
            }
            return false;
        }

        private bool BreaksMaxSingeCharSequence(StringBuilder variation)
        {
            var seqLength = 1;
            for (var i = 1; i < variation.Length; i++)
            {
                if (variation[i - 1] == variation[i])
                {
                    seqLength++;
                    if (seqLength > Options.AdjacentDuplicateMaxLength)
                    {
                        return true;
                    }
                }
                else
                {
                    seqLength = 1;
                }
            }
            return false;
        }

        private bool BreaksMaxCapitalLetterSequenceLength(StringBuilder variation)
        {
            var seqLength = 0;
            for (var i = 0; i < variation.Length; i++)
            {
                if (char.IsUpper(variation[i]))
                {
                    seqLength++;
                    if (seqLength > Options.CapitalAdjacentMaxLength)
                    {
                        return true;
                    }
                }
                else
                {
                    seqLength = 0;
                }
            }
            return false;
        }

        private bool BreaksMinCapitalLetterDistance(StringBuilder variation)
        {
            var startIndex = -1;
            for (var i = 0; i < variation.Length; i++)
            {
                if (char.IsUpper(variation[i]))
                {
                    if (startIndex < 0)
                    {
                        startIndex = i;
                    }
                    else
                    {
                        if (i - startIndex - 1 < Options.CapitalCharMinDistance)
                        {
                            return true;
                        }
                        startIndex = i;
                    }
                }
            }
            return false;
        }

        private bool BreaksByRegex(StringBuilder variation)
        {
            return _regex.IsMatch(variation.ToString());
        }

        private readonly IList<Func<StringBuilder, bool>> _checkers = new List<Func<StringBuilder, bool>>(3);
        private readonly Func<StringBuilder, bool> _breaksRestrictions = variation => false;
        private readonly Regex _regex;
    }
}
