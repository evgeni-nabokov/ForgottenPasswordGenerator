using System.Collections.Generic;
using System.Text;
using Lib.LetterMappers;

namespace Lib.PasswordSections
{
    public class PasswordPattern : IPasswordSection
    {
        public PasswordPattern(IEnumerable<IPasswordSection> sections, int maxSingeCharSequence = 3)
        {
            Sections = new List<IPasswordSection>(sections);
            MaxSingeCharSequence = maxSingeCharSequence;
        }

        public IReadOnlyList<IPasswordSection> Sections { get; }

        public int MaxSingeCharSequence { get; }

        public StringBuilder CurrentCombination { get; private set; }

        public int MaxLength
        {
            get
            {
                var result = 0;

                for (int i = 0; i < Sections.Count; i++)
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

                for (int i = 0; i < Sections.Count; i++)
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

            for (int i = 0; i < Sections.Count; i++)
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

        private StringBuilder GetCurrentCombination()
        {
            var builder = new StringBuilder(MaxLength);

            for (int i = 0; i < Sections.Count; i++)
            {
                builder.Append(Sections[i].CurrentCombination);
            }

            return builder;
        }

        public bool MoveToNextState()
        {
            for (int i = 0; i < Sections.Count; i++)
            {
                if (Sections[i].MoveToNextState()) return true;
            }

            var combination = GetCurrentCombination();
            if (HasMaxSingeCharSequence(combination))
            {
                return MoveToNextState();
            }
            
            CurrentCombination = combination;

            return false;
        }

        private bool HasMaxSingeCharSequence(StringBuilder b)
        {
            var seqLength = 1;
            for (int i = 1; i < b.Length; i++)
            {
                if (b[i - 1] == b[i])
                {
                    seqLength++;
                    if (seqLength >= MaxSingeCharSequence)
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
            for (int i = 0; i < Sections.Count; i++)
            {
                Sections[i].ResetState();
            }
        }
    }

    public class PasswordPatternBuilder
    {
        private IList<IPasswordSection> _sections;

        public PasswordPatternBuilder()
        {
            _sections = new List<IPasswordSection>();
        }

        public PasswordPatternBuilder(int size)
        {
            _sections = new List<IPasswordSection>(size);
        }

        public PasswordPatternBuilder AddSection(IPasswordSection section)
        {
            _sections.Add(section);
            return this;
        }

        public PasswordPatternBuilder AddFixedPasswordSection(
            string chars,
            CharCase charCase = CharCase.AsDefined,
            ILetterMapper mapper = null)
        {
            _sections.Add(new FixedPasswordSection(chars, charCase, mapper));
            return this;
        }

        public PasswordPatternBuilder AddArbitraryPasswordSection(
            string chars,
            int maxLength,
            int minLength = 1,
            CharCase charCase = CharCase.AsDefined,
            ILetterMapper mapper = null)
        {
            _sections.Add(new ArbitraryPasswordSection(chars, maxLength, minLength, charCase, mapper));
            return this;
        }

        public PasswordPattern Build()
        {
            return new PasswordPattern(_sections);
        }
    }
}
