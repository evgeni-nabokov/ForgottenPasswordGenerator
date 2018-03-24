using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Lib.PatternParser;

namespace Lib.PasswordSections
{
    public sealed class CompoundPasswordSection : IPasswordSection
    {
        public CompoundPasswordSection(
            string chars,
            CharCase charCase = CharCase.AsDefined)
        {
            OriginalChars = chars;
            CharCase = charCase;
            Sections = new ReadOnlyCollection<IPasswordSection>(BuildSections());
        }

        public string OriginalChars { get; }

        public CharCase CharCase { get; }

        public IReadOnlyList<IPasswordSection> Sections { get; }

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

        public string Current => BuildCurrent().ToString();

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

        public bool MoveNext()
        {
            for (var i = 0; i < Sections.Count; i++)
            {
                if (Sections[i].MoveNext())
                {
                    return true;
                }
                Sections[i].Reset();
            }

            return false;
        }

        public IEnumerable<string> GetVariations()
        {
            if (Sections == null || Sections.Count == 0) yield break;

            do
            {
                yield return Current;

            } while (MoveNext());
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

        private StringBuilder BuildCurrent()
        {
            var result = new StringBuilder(MaxLength);

            for (var i = 0; i < Sections.Count; i++)
            {
                result.Append(Sections[i].Current);
            }

            return result;
        }

        private IList<IPasswordSection> BuildSections()
        {
            var pieces = Parser.SplitIntoPieces(OriginalChars);
            var result = new List<IPasswordSection>(pieces.Count);
            for (var i = 0; i < pieces.Count; i++)
            {
                var p = pieces[i];
                if (p.Type == PatternPieceType.PlainString)
                {
                    result.Add(new FixedPasswordSection(p.Content, i == 0 ? MinLength : (int?)null, CharCase));
                }
                else
                {
                    result.Add(new StringListPasswordSection(Parser.SplitIntoElements(p.Content)));
                }
            }
            return result;
        }
    }
}
