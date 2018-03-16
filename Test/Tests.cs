using System;
using System.Text;
using Lib;
using Lib.CharMappers;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Xunit;

namespace Test
{
    public class Tests
    {
        [Fact]
        public void ApsFixedLengthVariationsStringTest()
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
        public void ApsVariableLengthVariationsStringTest()
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
        public void ApsNoSingleCharSequenceVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(1)
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("10");
            expected.AppendLine("01");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void ApsFixedLengthNoSingleCharSequenceVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(1)
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("10");
            expected.AppendLine("01");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void FpsFixedLengthUpperAndLowerVariationsStringTest()
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
        public void FpsVariableLengthUpperAndLowerVariationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(2)
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
        public void FpsVariableLengthCombinationCountTest()
        {
            var passwordPattern = new PasswordPatternBuilder(2)
                .AddFixedPasswordSection("ab", 0, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.Count;
            var expected = 7ul;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void FpsMaxCapitalLetterSequenceLengthStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(null, 1)
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
        public void FpsMinCapitalLetterDistanceStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(null, null, 1)
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
    }
}
