using System;
using System.Collections.Generic;
using System.Text;
using Lib.LetterMappers;

namespace Lib.PasswordSections
{
    public sealed class FixedPasswordSection : IPasswordSection
    {
        public FixedPasswordSection(
            string chars,
            CharCase charCase = CharCase.AsDefined,
            ILetterMapper mapper = null)
        {
            OriginalChars = chars;
            CharCase = charCase;
            CharMapper = mapper;
            BuildChars();
        }

        public int MaxLength => OriginalChars.Length;

        public int MinLength => OriginalChars.Length;

        public StringBuilder CurrentCombination => GetCurrentCombination();

        public string OriginalChars { get; }

        public CharCase CharCase { get; }

        public ILetterMapper CharMapper { get; }

        public ulong GetCombinationCount()
        {
            var result = 1ul;
            for (var i = 0; i < _chars.Length; i++)
            {
                result *= (ulong)_chars[i].Length;
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

        private StringBuilder GetCurrentCombination()
        {
            var builder = new StringBuilder(_chars.Length);

            for (var i = 0; i < _chars.Length; i++)
            {
                builder.Append(_chars[i][_permutationState[i]]);
            }

            return builder;
        }

        public bool MoveToNextState()
        {
            for (var i = 0; i < _chars.Length; i++)
            {
                if (_permutationState[i] < _chars[i].Length - 1)
                {
                    _permutationState[i] += 1;
                    return true;
                }
                _permutationState[i] = 0;
            }
            return false;
        }

        public void ResetState()
        {
            _permutationState = new int[MaxLength];
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

        private char[][] _chars;
        private int[] _permutationState;
    }
}
