using System;
using System.Collections.Generic;
using Lib.CharMappers;
using Lib.Suppressors;

namespace Lib.VariationGenerators
{
    public sealed class NumberRangeVariationGenerator : VariationGeneratorBase
    {
        public NumberRangeVariationGenerator(
            int minValue,
            int maxValue,
            int step = 1,
            string format = null,
            IList<ISuppressor> suppressors = null,
            ICharMapper mapper = null)
            : base(suppressors, mapper)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Step = step <= 0 ? 1 : step;
            Format = format;

            LoopCount = (ulong)Math.Floor((MaxValue - MinValue) / (double)Step);

            Reset();
        }

        public int MinValue { get; }

        public int MaxValue { get; }

        public int Step { get; }

        public string Format { get; }

        protected override bool GoToNextState()
        {
            if (_currentNumber + Step > MaxValue)
            {
                return false;
            }
            _currentNumber += Step;
            return true;
        }

        protected override string BuildVariation()
        {
            return _currentNumber.ToString(Format);
        }

        public override void Reset()
        {
            _currentNumber = MinValue;
            base.Reset();
        }

        private int _currentNumber;
    }
}
