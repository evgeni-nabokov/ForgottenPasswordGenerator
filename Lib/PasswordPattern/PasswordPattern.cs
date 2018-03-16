using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lib.PasswordSections;

namespace Lib.PasswordPattern
{
    public class PasswordPattern : IPasswordSection
    {
        public PasswordPattern(
            IEnumerable<IPasswordSection> sections,
            int? maxSingeCharSequenceLength = null,
            int? maxCapitalLetterSequenceLength = null,
            int? minCapitalLetterDistance = null)
        {
            Sections = new List<IPasswordSection>(sections);
            Checkers = new List<Func<StringBuilder, bool>>(3);

            MaxSingeCharSequenceLength = maxSingeCharSequenceLength < 1 ? null : maxSingeCharSequenceLength;
            if (MaxSingeCharSequenceLength.HasValue)
            {
                Checkers.Add(BreaksMaxSingeCharSequence);
            }

            MaxCapitalLetterSequenceLength = maxCapitalLetterSequenceLength < 1 ? null : maxCapitalLetterSequenceLength;
            if (MaxCapitalLetterSequenceLength.HasValue)
            {
                Checkers.Add(BreaksMaxCapitalLetterSequenceLength);
            }

            MinCapitalLetterDistance = minCapitalLetterDistance < 1 ? null : minCapitalLetterDistance;
            if (MinCapitalLetterDistance.HasValue)
            {
                Checkers.Add(BreaksMinCapitalLetterDistance);
            }

            if (Checkers.Count > 0)
            {
                _breaksRestrictions = BreaksAnyRestriction;
            }

            Init();
        }

        private IList<Func<StringBuilder, bool>> Checkers { get; }

        public IReadOnlyList<IPasswordSection> Sections { get; }

        public int? MaxSingeCharSequenceLength { get; }

        public int? MaxCapitalLetterSequenceLength { get; }

        public int? MinCapitalLetterDistance { get; }

        public int MaxLength
        {
            get
            {
                var result = 0;

                for (var i = 0; i < Sections.Count; i++)
                {
                    result += Sections[0].MaxLength;
                }

                return result;
            }
        }

        public int MinLength
        {
            get
            {
                var result = 0;

                for (var i = 0; i < Sections.Count; i++)
                {
                    result += Sections[i].MinLength;
                }

                return result;
            }
        }

        public ulong Count
        {
            get
            {
                var result = 1ul;

                if (Sections == null || Sections.Count == 0) return result;

                for (var i = 0; i < Sections.Count; i++)
                {
                    result *= Sections[i].Count;
                }

                return result;
            }
        }

        public IEnumerable<string> GetVariations()
        {
            if (Sections == null || Sections.Count == 0) yield break;

            do
            {
                yield return Current;

            } while (MoveNext());
        }

        public string Current => BuildCurrent().ToString();

        public bool MoveNext()
        {
            while (true)
            {
                var moved = false;
                for (var i = 0; i < Sections.Count; i++)
                {
                    if (Sections[i].MoveNext())
                    {
                        moved = true;
                        break;
                    }
                    Sections[i].Reset();
                }

                if (moved)
                {
                    if (!_breaksRestrictions(BuildCurrent()))
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public void Dispose()
        {
        }

        object IEnumerator.Current => Current;

        public void Reset()
        {
            for (var i = 0; i < Sections.Count; i++)
            {
                Sections[i].Reset();
            }
        }

        private void Init()
        {
            var current = BuildCurrent();

            if (_breaksRestrictions(current))
            {
                if (!MoveNext())
                {
                    throw new Exception("There are no combinations.");
                }
            }
        }

        private StringBuilder BuildCurrent()
        {
            var result = new StringBuilder(MaxLength);

            for (var i = 0; i < Sections.Count; i++)
            {
                result.Append(Sections[i].Current);
            }

            return result;
        }


        #region Checkers

        private readonly Func<StringBuilder, bool> _breaksRestrictions = variation => false;

        private bool BreaksAnyRestriction(StringBuilder variation)
        {
            for (var i = 0; i < Checkers.Count; i++)
            {
                if (Checkers[i](variation))
                {
                    return true;
                }
            }
            return false;
        }

        private bool BreaksMaxSingeCharSequence(StringBuilder variation)
        {
            var seqLength = 1;
            for (var i = 1; i < variation.Length; i++)
            {
                if (variation[i - 1] == variation[i])
                {
                    seqLength++;
                    if (seqLength > MaxSingeCharSequenceLength)
                    {
                        return true;
                    }
                }
                else
                {
                    seqLength = 1;
                }
            }
            return false;
        }

        private bool BreaksMaxCapitalLetterSequenceLength(StringBuilder variation)
        {
            var seqLength = 0;
            for (var i = 0; i < variation.Length; i++)
            {
                if (char.IsUpper(variation[i]))
                {
                    seqLength++;
                    if (seqLength > MaxCapitalLetterSequenceLength)
                    {
                        return true;
                    }
                }
                else
                {
                    seqLength = 0;
                }
            }
            return false;
        }

        private bool BreaksMinCapitalLetterDistance(StringBuilder variation)
        {
            var startIndex = -1;
            for (var i = 0; i < variation.Length; i++)
            {
                if (char.IsUpper(variation[i]))
                {
                    if (startIndex < 0)
                    {
                        startIndex = i;
                    }
                    else
                    {
                        if (i - startIndex - 1 < MinCapitalLetterDistance)
                        {
                            return true;
                        }
                        startIndex = i;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
