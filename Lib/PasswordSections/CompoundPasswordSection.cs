using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Lib.PasswordSections
{
    public sealed class CompoundPasswordSection : IPasswordSection
    {
        public CompoundPasswordSection(
            string chars,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined)
        {
            OriginalChars = chars;
            MinLength = NormalizeMinLength(minLength);
            CharCase = charCase;

            BuildSections();
        }

        public int MaxLength => OriginalChars.Length;

        public int MinLength { get; }

        public string OriginalChars { get; }

        public CharCase CharCase { get; }

        public ulong Count
        {
            get
            {
                var result = 0ul;
                for (var i = MinLength; i <= MaxLength; i++)
                {
                    var currentLengthCount = 1ul;
                    for (var j = 0; j < i; j++)
                    {
                        currentLengthCount *= (ulong)_chars[j].Length;
                    }
                    result += currentLengthCount;
                }

                return result;
            }
        }

        public IEnumerable<string> GetVariations()
        {
            do
            {
                yield return Current;

            } while (MoveNext());
        }

        public string Current
        {
            get
            {
                var builder = new StringBuilder(_currentLength);

                for (var i = 0; i < _currentLength; i++)
                {
                    builder.Append(_chars[i][_permutationState[i]]);
                }

                return builder.ToString();
            }
        }

        public bool MoveNext()
        {
            for (var i = 0; i < _currentLength; i++)
            {
                if (_permutationState[i] < _chars[i].Length - 1)
                {
                    _permutationState[i] += 1;
                    return true;
                }
                _permutationState[i] = 0;
            }

            if (_currentLength < MaxLength)
            {
                _currentLength++;
                _permutationState = new int[_currentLength];
                return true;
            }

            return false;
        }

        public void Reset()
        {
            _currentLength = MinLength;
            _permutationState = new int[_currentLength];
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current => Current;

        private void BuildSections()
        {
            var matches = Regex.Matches(OriginalChars, "({.*?})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            _sections = new List<IPasswordSection>(2 * matches.Count + 1);
            var firstIndex = 0;
            if (matches.Count > 0)
            {
                for (var i = 0; i < matches.Count; i++)
                {
                    var m = matches[i];
                    if (m.Index > 0)
                    {
                        var chars = OriginalChars.Substring(firstIndex, m.Index);
                        _sections.Add(new FixedPasswordSection(chars, MinLength, CharCase));
                    }
                    firstIndex = m.Index + m.Length;
                    _sections.Add(new StringListPasswordSection(m.Value.Split("|")));
                }
                if (firstIndex < OriginalChars.Length)
                {
                    var chars = OriginalChars.Substring(firstIndex);
                    _sections.Add(new FixedPasswordSection(chars, null, CharCase));
                }
            }
            else
            {
                _sections[0] = new FixedPasswordSection(OriginalChars, null, CharCase);
            }
        }

        private int NormalizeMinLength(int? minLength)
        {
            var result = minLength ?? MaxLength;
            return Math.Min(Math.Max(0, result), MaxLength);
        }

        private char[][] _chars;
        private int[] _permutationState;
        private int _currentLength;
        private IList<IPasswordSection> _sections;
    }
}
