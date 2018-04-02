using System;
using System.Collections.Generic;
using System.Text;
using Lib.CharMappers;
using Lib.CharsetGenerators;
using Lib.Suppressors;

namespace Lib.VariationGenerators
{
    public sealed class ArbitraryVariationGenerator : VariationGeneratorBase
    {
        public ArbitraryVariationGenerator(
            string chars,
            int minLength,
            int maxLength,
            CharCase charCase = CharCase.AsDefined,
            IList<ISuppressor> suppressors = null,
            ICharMapper mapper = null)
                : base(suppressors, mapper)
        {
            if (string.IsNullOrEmpty(chars))
            {
                throw new ArgumentNullException(nameof(chars), $"Empty parameter: {nameof(chars)}.");
            }
            OriginalChars = chars;
            MaxLength = maxLength < 1 ? 1 : maxLength;
            MinLength = NormalizeMinLength(minLength);
            CharCase = charCase;

            BuildCharsetGenerator();
            BuildChars();

            LoopCount = GetLoopCount();

            Reset();
        }

        public int MaxLength { get; }

        public int MinLength { get; }

        public int Size => Chars.Length;

        public string Chars { get; private set; }

        public string OriginalChars { get; }

        public CharCase CharCase { get; set; }

        protected override string BuildVariation()
        {
            var result = new StringBuilder(_currentLength);

            for (uint i = 0; i < _currentLength; i++)
            {
                result.Append(Chars[_permutationState[i]]);
            }

            return result.ToString();
        }

        protected override bool GoToNextState(out ulong passedLoops)
        {
            for (var i = 0; i < _currentLength; i++)
            {
                if (_permutationState[i] < Chars.Length - 1)
                {
                    _permutationState[i] += 1;
                    passedLoops = 1;
                    return true;
                }
                _permutationState[i] = 0;
            }

            if (_currentLength < MaxLength)
            {
                _currentLength++;
                _permutationState = new int[_currentLength];
                passedLoops = 1;
                return true;
            }

            passedLoops = 0;
            return false;
        }

        public override void Reset()
        {
            _currentLength = MinLength;
            _permutationState = new int[_currentLength];
            base.Reset();
        }

        private void BuildChars()
        {
            Chars = _charSetGenerator.GenerateCharset(OriginalChars);
        }

        private void BuildCharsetGenerator()
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

        private int NormalizeMinLength(int minLength)
        {
            return Math.Min(Math.Max(0, minLength), MaxLength);
        }

        private ulong GetLoopCount()
        {
            var result = 0ul;
            for (var i = MinLength; i <= MaxLength; i++)
            {
                result += (ulong)Math.Pow(Size, i);
            }
            return result;
        }

        private ICharsetGenerator _charSetGenerator;
        private int[] _permutationState;
        private int _currentLength;
    }
}
