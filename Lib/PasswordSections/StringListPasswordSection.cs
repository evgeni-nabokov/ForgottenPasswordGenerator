using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lib.PasswordSections
{
    public sealed class StringListPasswordSection : IPasswordSection
    {
        public StringListPasswordSection(
            IList<string> stringList)
        {
            StringList = new ReadOnlyCollection<string>(stringList.Distinct().ToList());

            var sortedByLength = StringList.OrderBy(x => x.Length).ToArray();
            MinLength = sortedByLength[0].Length;
            MaxLength = sortedByLength[sortedByLength.Length - 1].Length;

            Reset();
        }

        public int MinLength { get; }

        public int MaxLength { get; }

        public IReadOnlyList<string> StringList { get; }

        public ulong Count => (ulong)StringList.Count;

        public IEnumerable<string> GetVariations()
        {
            do
            {
                yield return Current;

            } while (MoveNext());
        }

        public string Current => StringList[_currentIndex];

        public bool MoveNext()
        {
            _currentIndex++;
            if (_currentIndex >= StringList.Count)
            {
                return false;
            }

            return true;
        }

        public void Reset()
        {
            _currentIndex = 0;
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current => Current;

        private int _currentIndex;
    }
}
