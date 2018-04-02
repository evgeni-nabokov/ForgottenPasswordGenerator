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
            Format = format ?? "N0";

            LoopCount = 1 + (ulong)Math.Floor((MaxValue - MinValue) / (double)Step);

            Reset();
        }

        public int MinValue { get; }

        public int MaxValue { get; }

        public int Step { get; }

        public string Format { get; }

        protected override bool GoToNextState(out ulong passedLoops)
        {
            if (_currentNumber + Step > MaxValue)
            {
                passedLoops = 0;
                return false;
            }
            passedLoops = 1;
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
