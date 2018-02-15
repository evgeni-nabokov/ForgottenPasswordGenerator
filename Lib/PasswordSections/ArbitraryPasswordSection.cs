using System;
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
            int minLength = 1,
            CharCase charCase = CharCase.AsDefined,
            ICharMapper mapper = null)
        {
            OriginalChars = chars;
            MaxLength = maxLength < 1 ? 1 : maxLength;
            MinLength = minLength < 1 ? 1 : minLength;
            CharCase = charCase;
            CharMapper = mapper;

            BuildGenerator();
            BuildChars();
            ResetState();
        }

        public int MaxLength { get; }

        public int MinLength { get; }

        public int Size => Chars.Length;

        public ICharMapper CharMapper { get; }

        public string Chars { get; private set; }

        public string OriginalChars { get; }

        public CharCase CharCase { get; set; }

        public ulong GetCombinationCount()
        {
            ulong result = 0;
            for (int i = MinLength; i <= MaxLength; i++)
            {
                result += (ulong)Math.Pow(Size, i);
            }
            return result;
        }

        public StringBuilder GetCurrentCombination()
        {
            var result = new StringBuilder(_currentLength);

            for (uint i = 0; i < _currentLength; i++)
            {
                result.Append(Chars[_permutationState[i]]);
            }

            return result;
        }

        public bool MoveToNextState()
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

        public void ResetState()
        {
            _currentLength = MinLength;
            _permutationState = new int[_currentLength];
        }

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

            if (CharMapper != null)
            {
                builder.AddLetterMapperGenerator(CharMapper);
            }

            builder.AddUniquenessCharGenerator();

            _charSetGenerator = builder.Build();
        }

        private ICharsetGenerator _charSetGenerator;
        private int[] _permutationState;
        private int _currentLength;
    }
}
