using System;
using System.Collections;
using System.Collections.Generic;

namespace Lib.PasswordSections
{
    public sealed class NumberRangePasswordSection : IPasswordSection
    {
        public NumberRangePasswordSection(
            int minValue,
            int maxValue,
            int? step)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Step = !step.HasValue || step == 0 ? 1 : step.Value;

            Reset();
        }

        public int MinLength => MinValue.ToString().Length;

        public int MaxLength => MaxValue.ToString().Length;

        public int MinValue { get; }

        public int MaxValue { get; }

        public int Step { get; }

        public ulong Count => (ulong)Math.Floor((double)((MaxValue - MinValue) / Step));

        public IEnumerable<string> GetVariations()
        {
            do
            {
                yield return Current;

            } while (MoveNext());
        }

        public string Current => _current.ToString();

        public bool MoveNext()
        {
            _current += Step;
            if (_current > MaxValue)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            _current = MinValue;
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current => Current;

        private int _current;
    }
}
