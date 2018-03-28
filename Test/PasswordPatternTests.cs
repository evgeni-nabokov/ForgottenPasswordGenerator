using System;
using System.Text;
using Lib;
using Lib.CharMappers;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Xunit;

namespace Test
{
    public class PasswordPatternTests
    {
        [Fact]
        public void Aps_FixedLengthVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("00");
            expected.AppendLine("10");
            expected.AppendLine("01");
            expected.AppendLine("11");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Aps_VariableLengthVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddArbitraryPasswordSection("01", 2)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("");
            expected.AppendLine("0");
            expected.AppendLine("1");
            expected.AppendLine("00");
            expected.AppendLine("10");
            expected.AppendLine("01");
            expected.AppendLine("11");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Aps_NoSingleCharSequenceVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetAdjacentDuplicateMaxLength(1)
                .AddArbitraryPasswordSection("01", 2, 0)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("");
            expected.AppendLine("0");
            expected.AppendLine("1");
            expected.AppendLine("10");
            expected.AppendLine("01");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Aps_FixedLengthNoSingleCharSequenceVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .SetAdjacentDuplicateMaxLength(1)
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("10");
            expected.AppendLine("01");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Fps_FixedLengthUpperAndLowerVariationsStringTest()
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
        public void Fps_VariableLengthUpperAndLowerVariationsStringTest()
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
        public void Fps_VariableLengthVariationCountTest()
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
        public void Fps_VariableLengthVariationNumberTest()
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
        public void Fps_MaxCapitalLetterSequenceLengthStringTest()
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
        public void Fps_MinCapitalLetterDistanceStringTest()
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
        public void Nrps_StringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddNumberRangePasswordSection(-2, 2)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("-2");
            expected.AppendLine("-1");
            expected.AppendLine("0");
            expected.AppendLine("1");
            expected.AppendLine("2");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Slps_StringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddStringListPasswordSection(new []{ "cat", "dog", "mouse" })
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("cat");
            expected.AppendLine("dog");
            expected.AppendLine("mouse");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Fps_RussianCharMapperStringTest()
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

        [Fact]
        public void Cps_SimplePatternStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddCompoundPasswordSection("{1|2|3}")
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("1");
            expected.AppendLine("2");
            expected.AppendLine("3");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Cps_SimplePatternStringTestWithDuplicates()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddCompoundPasswordSection("{1|2||3|}")
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("1");
            expected.AppendLine("2");
            expected.AppendLine("");
            expected.AppendLine("3");
            Assert.Equal(expected.ToString(), actual);
        }
    }
}
