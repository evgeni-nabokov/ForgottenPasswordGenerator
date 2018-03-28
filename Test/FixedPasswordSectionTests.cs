using System;
using System.Text;
using Lib;
using Lib.CharMappers;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Xunit;

namespace Test
{
    public class FixedPasswordSectionTests
    {
        [Fact]
        public void FixedLengthUpperAndLowerVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddFixedPasswordSection("ab", null, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("ab");
            expected.AppendLine("Ab");
            expected.AppendLine("aB");
            expected.AppendLine("AB");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void VariableLengthUpperAndLowerVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetAdjacentDuplicateMaxLength(1)
                .AddFixedPasswordSection("ab", 0, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("");
            expected.AppendLine("a");
            expected.AppendLine("A");
            expected.AppendLine("ab");
            expected.AppendLine("Ab");
            expected.AppendLine("aB");
            expected.AppendLine("AB");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void VariableLengthVariationCountTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetAdjacentDuplicateMaxLength(2)
                .AddFixedPasswordSection("ab", 0, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.Count;
            var expected = 7ul;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VariableLengthVariationNumberTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetAdjacentDuplicateMaxLength(2)
                .AddFixedPasswordSection("ab", 0, CharCase.UpperAndLower)
                .Build();

            passwordPattern.GetVariationsString();
            var actual = passwordPattern.CurrentNumber;
            var expected = 7ul;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void MaxCapitalLetterSequenceLengthStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetCapitalAdjacentMaxLength(1)
                .AddFixedPasswordSection("abc", 3, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("abc");
            expected.AppendLine("Abc");
            expected.AppendLine("aBc");
            expected.AppendLine("abC");
            expected.AppendLine("AbC");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void MinCapitalLetterDistanceStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetCapitalCharMinDistance(1)
                .AddFixedPasswordSection("abc", 3, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("abc");
            expected.AppendLine("Abc");
            expected.AppendLine("aBc");
            expected.AppendLine("abC");
            expected.AppendLine("AbC");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void RussianCharMapperStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetCharMapper(new RussianToEnglishMapper())
                .AddFixedPasswordSection("ÈˆÛÍÂÌ„¯˘Áı˙Ù˚‚‡ÔÓÎ‰Ê˝ˇ˜ÒÏËÚ¸·˛∏…÷” ≈Õ√ÿŸ«’⁄‘€¬¿œ–ŒÀƒ∆›ﬂ◊—Ã»“‹¡ﬁ®")
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("qwertyuiop[]asdfghjkl;'zxcvbnm,.`QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>~");
            Assert.Equal(expected.ToString(), actual);
        }
    }
}
