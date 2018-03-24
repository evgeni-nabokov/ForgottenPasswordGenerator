using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Lib.PatternParser;

namespace Lib.PasswordSections
{
    public sealed class CompoundPasswordSection : IPasswordSection
    {
        public CompoundPasswordSection(
            string chars,
            int? minLength = null,
            CharCase charCase = CharCase.AsDefined)
        {
            OriginalChars = chars;
            CharCase = charCase;
            BuildSections();
            MinLength = NormalizeMinLength(minLength);

            if (minLength.HasValue)
            {
                _checkers.Add(BreaksMinLength);
            }

            if (_checkers.Count > 0)
            {
                _breaksRestrictions = BreaksAnyRestriction;
            }
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

        public int MinLength { get; }

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
            //Init();
        }

        //private void Init()
        //{
        //    var current = BuildCurrent();
        //    LoopNumber = 1;

        //    if (_breaksRestrictions(current))
        //    {
        //        if (!MoveNext())
        //        {
        //            throw new Exception("There are no combinations.");
        //        }
        //    }

        //    CurrentNumber = 1;
        //}

        private StringBuilder BuildCurrent()
        {
            var result = new StringBuilder(MaxLength);

            for (var i = 0; i < Sections.Count; i++)
            {
                result.Append(Sections[i].Current);
            }

            return result;
        }

        private void BuildSections()
        {
            var pieces = Parser.SplitIntoPieces(OriginalChars);
            var sections = new List<IPasswordSection>(pieces.Count);
            for (var i = 0; i < pieces.Count; i++)
            {
                var p = pieces[i];
                if (p.Type == PatternPieceType.PlainString)
                {
                    sections.Add(new FixedPasswordSection(p.Content, MinLength, CharCase));
                }
                else
                {
                    sections.Add(new StringListPasswordSection(p.Content.Split("|")));
                }
            }
        }

        private int NormalizeMinLength(int? minLength)
        {
            var result = minLength ?? MaxLength;
            return Math.Min(Math.Max(0, result), MaxLength);
        }


        #region Checkers

        private readonly IList<Func<StringBuilder, bool>> _checkers = new List<Func<StringBuilder, bool>>(3);
        private readonly Func<StringBuilder, bool> _breaksRestrictions = variation => false;

        private bool BreaksAnyRestriction(StringBuilder variation)
        {
            for (var i = 0; i < _checkers.Count; i++)
            {
                if (_checkers[i](variation))
                {
                    return true;
                }
            }
            return false;
        }

        private bool BreaksMinLength(StringBuilder variation)
        {
            return variation.Length < MinLength;
        }
        #endregion
    }

    
}
