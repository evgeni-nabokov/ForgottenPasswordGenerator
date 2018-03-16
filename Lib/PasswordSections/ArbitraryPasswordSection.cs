using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lib.CharsetGenerators;
using Lib.CharMappers;

namespace Lib.PasswordSections
{
    public sealed class ArbitraryPasswordSection : IPasswordSection
    {
        public ArbitraryPasswordSection(
            string chars,
            int maxLength,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined)
        {
            OriginalChars = chars;
            MaxLength = maxLength < 1 ? 1 : maxLength;
            MinLength = NormalizeMinLength(minLength);
            CharCase = charCase;

            BuildGenerator();
            BuildChars();
            Reset();
        }

        public int MaxLength { get; }

        public int MinLength { get; }

        public int Size => Chars.Length;

        public string Chars { get; private set; }

        public string OriginalChars { get; }

        public CharCase CharCase { get; set; }

        public ulong Count
        {
            get
            {
                ulong result = 0;
                for (int i = MinLength; i <= MaxLength; i++)
                {
                    result += (ulong) Math.Pow(Size, i);
                }
                return result;
            }
        }

        public string Current
        {
            get
            {
                var result = new StringBuilder(_currentLength);

                for (uint i = 0; i < _currentLength; i++)
                {
                    result.Append(Chars[_permutationState[i]]);
                }

                return result.ToString();
            }
        }

        public bool MoveNext()
        {
            for (var i = 0; i < _currentLength; i++)
            {
                if (_permutationState[i] < Chars.Length - 1)
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

        public IEnumerable<string> GetVariations()
        {
            do
            {
                yield return Current;

            } while (MoveNext());
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current => Current;

        private void BuildChars()
        {
            Chars = _charSetGenerator.GenerateCharset(OriginalChars);
        }

        private void BuildGenerator()
        {
            var builder = new CharsetGeneratorBuilder();

            switch (CharCase)
            {
                case CharCase.Lower:
                    builder.AddLowerCaseGenerator();
                    break;
                case CharCase.Upper:
                    builder.AddUpperCaseGenerator();
                    break;
                case CharCase.UpperAndLower:
                    builder.AddUpperAndLowerCaseGenerator();
                    break;
            }

            builder.AddUniquenessCharGenerator();

            _charSetGenerator = builder.Build();
        }

        private int NormalizeMinLength(int? minLength)
        {
            var result = minLength ?? 0;
            return Math.Min(Math.Max(0, result), MaxLength);
        }

        private ICharsetGenerator _charSetGenerator;
        private int[] _permutationState;
        private int _currentLength;
    }
}
