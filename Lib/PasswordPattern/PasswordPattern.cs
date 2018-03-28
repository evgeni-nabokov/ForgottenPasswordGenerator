using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lib.CharMappers;
using Lib.PasswordPattern.Suppression;
using Lib.PasswordSections;

[assembly: InternalsVisibleTo("Test")]

namespace Lib.PasswordPattern
{
    public class PasswordPattern : IPasswordSection
    {
        public PasswordPattern(
            IEnumerable<IPasswordSection> sections,
            ISuppressor suppressor,
            ICharMapper mapper = null)
        {
            Sections = new List<IPasswordSection>(sections);
            Suppressor = suppressor;
            CharMapper = mapper;

            if (Suppressor != null)
            {
                _breaksRestrictions = Suppressor.BreaksRestrictions;
            }
            
            if (CharMapper != null)
            {
                _processors.Add(MapCharacters);
                _finalProcess = ProcessVariation;
            }

            Init();
        }

        public readonly IReadOnlyList<IPasswordSection> Sections;

        public readonly ISuppressor Suppressor;

        public readonly ICharMapper CharMapper;

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

        public ulong CurrentNumber { get; private set; }
        
        public IEnumerable<string> GetVariations()
        {
            if (Sections == null || Sections.Count == 0) yield break;

            do
            {
                yield return Current;

            } while (MoveNext());
        }

        public string Current => _finalProcess(BuildCurrent()).ToString();

        public ulong LoopNumber { get; private set; }

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
                    LoopNumber++;
                    if (!_breaksRestrictions(BuildCurrent()))
                    {
                        CurrentNumber++;
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
            Init();
        }

        private void Init()
        {
            var current = BuildCurrent();
            LoopNumber = 1;

            if (_breaksRestrictions(current))
            {
                if (!MoveNext())
                {
                    throw new Exception("There are no variations.");
                }
            }

            CurrentNumber = 1;
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


        #region Post-build processors

        private readonly IList<Func<StringBuilder, StringBuilder>> _processors = new List<Func<StringBuilder, StringBuilder>>(1);
        private readonly Func<StringBuilder, StringBuilder> _finalProcess = variation => variation;

        private StringBuilder ProcessVariation(StringBuilder variation)
        {
            for (var i = 0; i < _processors.Count; i++)
            {
                variation = _processors[i](variation);
            }
            return variation;
        }

        private StringBuilder MapCharacters(StringBuilder variation)
        {
            for (var i = 0; i < variation.Length; i++)
            {
                if (CharMapper.TryGetLetter(variation[i], out var convertedChar))
                {
                    variation[i] = convertedChar;
                }
            }
            return variation;
        }

        #endregion

        private readonly Func<StringBuilder, bool> _breaksRestrictions = variation => false;
    }
}
