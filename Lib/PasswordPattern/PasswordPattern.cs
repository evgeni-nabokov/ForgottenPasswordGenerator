using System;
using System.Collections.Generic;
using System.Text;
using Lib.PasswordSections;

namespace Lib.PasswordPattern
{
    public class PasswordPattern : IPasswordSection
    {
        public PasswordPattern(IEnumerable<IPasswordSection> sections, int? maxSingeCharSequence = null)
        {
            Sections = new List<IPasswordSection>(sections);
            MaxSingeCharSequence = maxSingeCharSequence < 1 ? null : maxSingeCharSequence;
        }

        public IReadOnlyList<IPasswordSection> Sections { get; }

        public int? MaxSingeCharSequence { get; }

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
                    result += Sections[0].MinLength;
                }

                return result;
            }
        }

        public ulong GetCombinationCount()
        {
            var result = 1ul;

            if (Sections == null || Sections.Count == 0) return result;

            for (var i = 0; i < Sections.Count; i++)
            {
                result *= Sections[i].GetCombinationCount();
            }

            return result;
        }

        public IEnumerable<string> GetCombinations()
        {
            if (Sections == null || Sections.Count == 0) yield break;

            do
            {
                yield return GetCurrentCombination().ToString();

            } while (MoveToNextState());
        }

        public StringBuilder GetCurrentCombination()
        {
            var result = BuildCurrentCombination();

            if (HasMaxSingeCharSequence(result))
            {
                if (MoveToNextState())
                {
                    result = BuildCurrentCombination();
                }
                else
                {
                    throw new Exception("There is no any combination.");
                }
            }
            else
            {
                return result;
            }

            return result;
        }

        public bool MoveToNextState()
        {
            while (true)
            {
                var moved = false;
                for (var i = 0; i < Sections.Count; i++)
                {
                    if (Sections[i].MoveToNextState())
                    {
                        moved = true;
                        break;
                    }
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

        private bool HasMaxSingeCharSequence(StringBuilder b)
        {
            if (!MaxSingeCharSequence.HasValue) return false;

            var seqLength = 1;
            for (int i = 1; i < b.Length; i++)
            {
                if (b[i - 1] == b[i])
                {
                    seqLength++;
                    if (seqLength > MaxSingeCharSequence)
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

        public void ResetState()
        {
            for (var i = 0; i < Sections.Count; i++)
            {
                Sections[i].ResetState();
            }
        }

        private StringBuilder BuildCurrentCombination()
        {
            var result = new StringBuilder(MaxLength);

            for (var i = 0; i < Sections.Count; i++)
            {
                result.Append(Sections[i].GetCurrentCombination());
            }

            return result;
        }
    }

    
}
