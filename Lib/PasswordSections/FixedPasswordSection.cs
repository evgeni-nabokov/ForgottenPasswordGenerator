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
            _originalChars = chars;
            _charCase = charCase;
            _languageLettersConverter = mapper;
            BuildChars();
        }

        public int MaxLength => _originalChars.Length;
        public int MinLength => _originalChars.Length;

        private string _originalChars;
        public string OriginalChars
        {
            get => _originalChars;
            set
            {
                _originalChars = value ?? string.Empty;
                BuildChars();
            }
        }

        private CharCase _charCase;
        public CharCase CharCase
        {
            get => _charCase;
            set
            {
                _charCase = value;
                BuildChars();
            }
        }

        private readonly ILetterMapper _languageLettersConverter;
        public ILetterMapper LanguageLettersConverter => _languageLettersConverter;

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

        public StringBuilder GetCurrentCombination()
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

            if (_originalChars.Length == 0)
            {
                _chars = new char[0][];
                return;
            }

            _chars = new char[_originalChars.Length][];
            var placeIndex = 0;
            foreach (var c in _originalChars)
            {
                if (char.IsLetter(c))
                {
                    switch (_charCase)
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

            if (_languageLettersConverter != null)
            {
                for (var i = 0; i < _chars.Length; i++)
                {
                    for (var j = 0; j < _chars[i].Length; j++)
                    {
                        if (_languageLettersConverter.TryGetLetter(_chars[i][j], out var convertedChar))
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
