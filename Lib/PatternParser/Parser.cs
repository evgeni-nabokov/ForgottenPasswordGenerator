using System.Collections.Generic;
using System.Text;

namespace Lib.PatternParser
{
    internal static class Parser
    {
        public static IList<PatternPiece> SplitIntoPieces(string pattern)
        {
            var result = new List<PatternPiece>(pattern.Length);

            var braceFlag = false;
            var slashFlag = false;
            var piece = new StringBuilder(pattern.Length);
            var sectionStartIndex = 0;
            for (var i = 0; i < pattern.Length; i++)
            {
                var c = pattern[i];

                if (c == '\\')
                {
                    if (slashFlag)
                    {
                        piece.Append(c);
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
                        if (piece.Length > 0)
                        {
                            result.Add(new PatternPiece(piece.ToString(), sectionStartIndex, PatternPieceType.PlainString));
                            piece.Clear();
                        }

                        braceFlag = true;
                        sectionStartIndex = i;
                    }
                    piece.Append(c);
                }
                else if (c == '}')
                {
                    if (slashFlag || !braceFlag)
                    {
                        piece.Append(c);
                    }
                    else
                    {
                        if (piece.Length > 1)
                        {
                            result.Add(new PatternPiece(piece.ToString(1), sectionStartIndex, PatternPieceType.BraceContent));
                        }
                        piece.Clear();
                        braceFlag = false;
                        sectionStartIndex = i + 1;
                    }
                }
                else
                {
                    piece.Append(c);
                }

                slashFlag = false;
            }

            if (piece.Length > 0)
            {
                result.Add(new PatternPiece(piece.ToString(), sectionStartIndex, PatternPieceType.PlainString));
            }

            return CombineAdjacentPlainTextPieces(result);
        }

        private static IList<PatternPiece> CombineAdjacentPlainTextPieces(IList<PatternPiece> sections)
        {
            var result = new List<PatternPiece>(sections.Count);

            var accum = new StringBuilder();
            var accumStartIndex = 0;
            for (var i = 0; i < sections.Count; i++)
            {
                var p = sections[i];
                if (p.Type == PatternPieceType.BraceContent)
                {
                    if (accum.Length > 0)
                    {
                        result.Add(new PatternPiece(accum.ToString(), accumStartIndex, PatternPieceType.PlainString));
                        accum.Clear();
                    }
                    result.Add(p);
                }
                else
                {
                    if (accum.Length == 0)
                    {
                        accumStartIndex = p.StartIndex;
                    }
                    accum.Append(p.Content);
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
