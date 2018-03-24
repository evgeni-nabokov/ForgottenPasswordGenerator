using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.PatternParser
{
    internal static class PatternParser
    {
        public static IList<PatternPiece> SplitIntoSections(string pattern)
        {
            var result = new List<PatternPiece>(pattern.Length);

            var braceFlag = false;
            var slashFlag = false;
            var section = new StringBuilder(pattern.Length);
            var sectionStartIndex = 0;
            for (var i = 0; i < pattern.Length; i++)
            {
                var c = pattern[i];

                if (c == '\\')
                {
                    if (slashFlag)
                    {
                        section.Append(c);
                        slashFlag = false;
                    }
                    else
                    {
                        slashFlag = true;
                    }
                    continue;
                }

                if (c == '{')
                {
                    if (!slashFlag && !braceFlag)
                    {
                        if (section.Length > 0)
                        {
                            result.Add(new PatternPiece(section.ToString(), sectionStartIndex, PatternPieceType.PlainString));
                            section.Clear();
                        }

                        braceFlag = true;
                        sectionStartIndex = i;
                    }
                    section.Append(c);
                }
                else if (c == '}')
                {
                    if (slashFlag || !braceFlag)
                    {
                        section.Append(c);
                    }
                    else
                    {
                        if (section.Length > 1)
                        {
                            result.Add(new PatternPiece(section.ToString(1), sectionStartIndex, PatternPieceType.BraceContent));
                        }
                        section.Clear();
                        braceFlag = false;
                        sectionStartIndex = i + 1;
                    }
                }
                else
                {
                    section.Append(c);
                }

                slashFlag = false;
            }

            if (section.Length > 0)
            {
                result.Add(new PatternPiece(section.ToString(), sectionStartIndex, PatternPieceType.PlainString));
            }

            return CombineAdjacentPlainTextSections(result);
        }

        private static IList<PatternPiece> CombineAdjacentPlainTextSections(IList<PatternPiece> sections)
        {
            var result = new List<PatternPiece>(sections.Count);

            var accum = new StringBuilder();
            var accumStartIndex = 0;
            for (var i = 0; i < sections.Count; i++)
            {
                var s = sections[i];
                if (s.Type == PatternPieceType.BraceContent)
                {
                    if (accum.Length > 0)
                    {
                        result.Add(new PatternPiece(accum.ToString(), accumStartIndex, PatternPieceType.PlainString));
                        accum.Clear();
                    }
                    result.Add(s);
                }
                else
                {
                    if (accum.Length == 0)
                    {
                        accumStartIndex = s.StartIndex;
                    }
                    accum.Append(s.Content);
                }
            }

            if (accum.Length > 0)
            {
                result.Add(new PatternPiece(accum.ToString(), accumStartIndex, PatternPieceType.PlainString));
            }

            return result;
        }

        private static string ToString(this StringBuilder sb, int startIndex)
        {
            return sb.ToString(startIndex, sb.Length - startIndex);
        }
    }
}
