using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lib.PasswordSections
{
    public sealed class FixedPasswordSection : IPasswordSection
    {
        public FixedPasswordSection(
            string chars,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined)
        {
            OriginalChars = chars;
            MinLength = NormalizeMinLength(minLength);
            CharCase = charCase;

            BuildChars();
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
                        currentLengthCount *= (ulong) _chars[j].Length;
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

        private void BuildChars()
        {
            Reset();

            if (OriginalChars.Length == 0)
            {
                _chars = new char[0][];
                return;
            }

            _chars = new char[OriginalChars.Length][];
            var placeIndex = 0;
            foreach (var c in OriginalChars)
            {
                if (CharCase != CharCase.AsDefined && char.IsLetter(c))
                {
                    switch (CharCase)
                    {
                        case CharCase.Lower:
                            _chars[placeIndex] = new[] { char.ToLower(c) };
                            break;
                        case CharCase.Upper:
                            _chars[placeIndex] = new[] { char.ToUpper(c) };
                            break;
                        case CharCase.UpperAndLower:
                            _chars[placeIndex] = new[] { char.ToLower(c), char.ToUpper(c) };
                            break;
                    }
                }
                else
                {
                    _chars[placeIndex] = new[] { c };
                }
                placeIndex++;
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
    }
}
