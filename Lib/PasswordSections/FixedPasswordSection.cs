using System;
using System.Collections.Generic;
using System.Text;
using Lib.CharMappers;

namespace Lib.PasswordSections
{
    public sealed class FixedPasswordSection : IPasswordSection
    {
        public FixedPasswordSection(
            string chars,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined,
            ICharMapper mapper = null)
        {
            OriginalChars = chars;
            MinLength = NormalizeMinLength(minLength);
            CharCase = charCase;
            CharMapper = mapper;
            BuildChars();
        }

        public int MaxLength => OriginalChars.Length;

        public int MinLength { get; }

        public string OriginalChars { get; }

        public CharCase CharCase { get; }

        public ICharMapper CharMapper { get; }

        public ulong GetCombinationCount()
        {
            var result = 0ul;
            for(var i = MinLength; i <= MaxLength; i++)
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

        public IEnumerable<string> GetCombinations()
        {
            do
            {
                yield return GetCurrentCombination().ToString();

            } while (MoveToNextState());
        }

        public StringBuilder GetCurrentCombination()
        {
            var builder = new StringBuilder(_currentLength);

            for (var i = 0; i < _currentLength; i++)
            {
                builder.Append(_chars[i][_permutationState[i]]);
            }

            return builder;
        }

        public bool MoveToNextState()
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

        public void ResetState()
        {
            _currentLength = MinLength;
            _permutationState = new int[_currentLength];
        }

        private void BuildChars()
        {
            ResetState();

            if (OriginalChars.Length == 0)
            {
                _chars = new char[0][];
                return;
            }

            _chars = new char[OriginalChars.Length][];
            var placeIndex = 0;
            foreach (var c in OriginalChars)
            {
                if (char.IsLetter(c))
                {
                    switch (CharCase)
                    {
                        case CharCase.AsDefined:
                            _chars[placeIndex] = new[] { c };
                            break;
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

            if (CharMapper != null)
            {
                for (var i = 0; i < _chars.Length; i++)
                {
                    for (var j = 0; j < _chars[i].Length; j++)
                    {
                        if (CharMapper.TryGetLetter(_chars[i][j], out var convertedChar))
                        {
                            _chars[i][j] = convertedChar;
                        }
                    }
                }
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
