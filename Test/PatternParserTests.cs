using System.Collections.Generic;
using System.Text;
using Lib.PatternParsers;
using Lib.VariationGenerators;
using Xunit;

namespace Test
{
    public class PatternParserTests
    {
        [Fact]
        public void SplitIntoPiecesSimpleTest()
        {
            var actual = PatternParser.SplitIntoPieces("a{1}b");

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
            var actual = PatternParser.SplitIntoPieces("{}a}b{{1}}c{d{\\\\");
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
            var actual = PatternParser.SplitIntoElements("a|b|c|d");
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
            var actual = PatternParser.SplitIntoElements("|a|b||c|d|\\||");
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

        [Fact]
        public void SimplePatternStringTest()
        {
            var generator = PatternParser.Parse("{1|2|3}");

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("1");
            expected.AppendLine("2");
            expected.AppendLine("3");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void PatternWithDuplicatesStringTest()
        {
            var generator = PatternParser.Parse("{1|2||3|}");

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("1");
            expected.AppendLine("2");
            expected.AppendLine("");
            expected.AppendLine("3");

            Assert.Equal(expected.ToString(), actual);
        }
    }
}
