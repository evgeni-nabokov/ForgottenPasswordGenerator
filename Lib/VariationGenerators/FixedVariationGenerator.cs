using System;
using System.Collections.Generic;
using System.Text;
using Lib.CharMappers;
using Lib.Suppressors;

namespace Lib.VariationGenerators
{
    public sealed class FixedVariationGenerator : VariationGeneratorBase
    {
        public FixedVariationGenerator(
            string chars,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined,
            IList<ISuppressor> suppressors = null,
            ICharMapper mapper = null)
            : base(suppressors, mapper)
        {
            if (string.IsNullOrEmpty(chars))
            {
                throw new ArgumentNullException(nameof(chars), $"Empty parameter: {nameof(chars)}");
            }

            OriginalChars = chars;
            MinLength = NormalizeMinLength(minLength);
            CharCase = charCase;

            BuildChars();
            Reset();

            LoopCount = GetLoopCount();
        }

        public int MaxLength => OriginalChars.Length;

        public int MinLength { get; }

        public string OriginalChars { get; }

        public CharCase CharCase { get; }

        protected override string BuildVariation()
        {
            var result = new StringBuilder(_currentLength);

            for (var i = 0; i < _currentLength; i++)
            {
                result.Append(_chars[i][_permutationState[i]]);
            }

            return result.ToString();
        }

        protected override bool GoToNextState()
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

        public override void Reset()
        {
            _currentLength = MinLength;
            _permutationState = new int[_currentLength];
            base.Reset();
        }

        private void BuildChars()
        {
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
            minLength = minLength ?? MaxLength;
            return Math.Min(Math.Max(0, minLength.Value), MaxLength);
        }

        private ulong GetLoopCount()
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

        private char[][] _chars;
        private int[] _permutationState;
        private int _currentLength;
    }
}
