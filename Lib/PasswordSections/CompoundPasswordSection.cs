using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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

        //private void BuildSections()
        //{
        //    var matches = Regex.Matches(OriginalChars, "({.*?})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //    var sections = new List<IPasswordSection>(2 * matches.Count + 1);
        //    var firstIndex = 0;
        //    if (matches.Count > 0)
        //    {
        //        for (var i = 0; i < matches.Count; i++)
        //        {
        //            var m = matches[i];
        //            if (m.Index > 0)
        //            {
        //                var chars = OriginalChars.Substring(firstIndex, m.Index);
        //                sections.Add(new FixedPasswordSection(chars, MinLength, CharCase));
        //            }
        //            firstIndex = m.Index + m.Length;
        //            var value = m.Value.Substring(1, m.Value.Length - 2);
        //            sections.Add(new StringListPasswordSection(value.Split("|")));
        //        }
        //        if (firstIndex < OriginalChars.Length)
        //        {
        //            var chars = OriginalChars.Substring(firstIndex);
        //            sections.Add(new FixedPasswordSection(chars, null, CharCase));
        //        }
        //    }
        //    else
        //    {
        //        sections[0] = new FixedPasswordSection(OriginalChars, null, CharCase);
        //    }
        //}

        private void BuildSections()
        {
            

            var matches = Regex.Matches(OriginalChars, "({.*?})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            var sections = new List<IPasswordSection>(2 * matches.Count + 1);
            var firstIndex = 0;
            if (matches.Count > 0)
            {
                for (var i = 0; i < matches.Count; i++)
                {
                    var m = matches[i];
                    if (m.Index > 0)
                    {
                        var chars = OriginalChars.Substring(firstIndex, m.Index);
                        sections.Add(new FixedPasswordSection(chars, MinLength, CharCase));
                    }
                    firstIndex = m.Index + m.Length;
                    var value = m.Value.Substring(1, m.Value.Length - 2);
                    sections.Add(new StringListPasswordSection(value.Split("|")));
                }
                if (firstIndex < OriginalChars.Length)
                {
                    var chars = OriginalChars.Substring(firstIndex);
                    sections.Add(new FixedPasswordSection(chars, null, CharCase));
                }
            }
            else
            {
                sections[0] = new FixedPasswordSection(OriginalChars, null, CharCase);
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
