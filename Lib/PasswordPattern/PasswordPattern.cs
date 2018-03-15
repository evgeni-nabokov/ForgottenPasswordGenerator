using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lib.PasswordSections;

namespace Lib.PasswordPattern
{
    public class PasswordPattern : IPasswordSection
    {
        public PasswordPattern(IEnumerable<IPasswordSection> sections, int? maxSingeCharSequenceLength = null)
        {
            Sections = new List<IPasswordSection>(sections);
            MaxSingeCharSequenceLength = maxSingeCharSequenceLength < 1 ? null : maxSingeCharSequenceLength;
        }

        public IReadOnlyList<IPasswordSection> Sections { get; }

        public int? MaxSingeCharSequenceLength { get; }

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

        public string Current
        {
            get
            {
                var result = BuildCurrentCombination();

                if (HasMaxSingeCharSequence(result))
                {
                    if (MoveNext())
                    {
                        result = BuildCurrentCombination();
                    }
                    else
                    {
                        throw new Exception("There are no more combination.");
                    }
                }
                else
                {
                    return result.ToString();
                }

                return result.ToString();
            }
        }

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
                    var combination = BuildCurrentCombination();

                    if (!HasMaxSingeCharSequence(combination))
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

        private bool HasMaxSingeCharSequence(StringBuilder b)
        {
            if (!MaxSingeCharSequenceLength.HasValue) return false;

            var seqLength = 1;
            for (int i = 1; i < b.Length; i++)
            {
                if (b[i - 1] == b[i])
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

        public void Reset()
        {
            for (var i = 0; i < Sections.Count; i++)
            {
                Sections[i].Reset();
            }
        }

        private StringBuilder BuildCurrentCombination()
        {
            var result = new StringBuilder(MaxLength);

            for (var i = 0; i < Sections.Count; i++)
            {
                result.Append(Sections[i].Current);
            }

            return result;
        }
    }

    
}
