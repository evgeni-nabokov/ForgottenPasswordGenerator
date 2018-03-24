using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Lib.PasswordSections;
using Lib.PatternParser;
using Xunit;

namespace Test
{
    public class PatternParserTests
    {
        [Fact]
        public void SimplePatternTest()
        {
            var actual = PatternParser.SplitIntoSections("a{1}b");
            var expected = new List<PatternPiece>
            {
                new PatternPiece("a", 0, PatternPieceType.PlainString),
                new PatternPiece("1", 1, PatternPieceType.BraceContent),
                new PatternPiece("b", 4, PatternPieceType.PlainString)
            };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComplexPatternTest()
        {
            var actual = PatternParser.SplitIntoSections("{}a}b{{1}}c{d{");
            var expected = new List<PatternPiece>
            {
                new PatternPiece("a}b", 2, PatternPieceType.PlainString),
                new PatternPiece("{1", 5, PatternPieceType.BraceContent),
                new PatternPiece("}c{d{", 9, PatternPieceType.PlainString)
            };
            Assert.Equal(expected, actual);
        }
    }
}
