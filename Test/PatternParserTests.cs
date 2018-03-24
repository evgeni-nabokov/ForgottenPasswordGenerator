using System;
using System.Collections.Generic;
using Lib.PatternParser;
using Xunit;

namespace Test
{
    public class PatternParserTests
    {
        [Fact]
        public void SplitIntoPiecesSimpleTest()
        {
            var actual = Parser.SplitIntoPieces("a{1}b");
            var expected = new List<PatternPiece>
            {
                new PatternPiece("a", 0, PatternPieceType.PlainString),
                new PatternPiece("1", 1, PatternPieceType.BraceContent),
                new PatternPiece("b", 4, PatternPieceType.PlainString)
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SplitIntoPiecesComplexTest()
        {
            var actual = Parser.SplitIntoPieces("{}a}b{{1}}c{d{\\\\");
            var expected = new List<PatternPiece>
            {
                new PatternPiece("a}b", 2, PatternPieceType.PlainString),
                new PatternPiece("{1", 5, PatternPieceType.BraceContent),
                new PatternPiece("}c{d{\\", 9, PatternPieceType.PlainString)
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SplitIntoElementsSimpleTest()
        {
            var actual = Parser.SplitIntoElements("a|b|c|d");
            var expected = new List<string>
            {
                "a",
                "b",
                "c",
                "d"
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SplitIntoElementsComplexTest()
        {
            var actual = Parser.SplitIntoElements("|a|b||c|d|\\||");
            var expected = new List<string>
            {
                "",
                "a",
                "b",
                "",
                "c",
                "d",
                "|",
                ""
            };
            Assert.Equal(expected, actual);
        }
    }
}
