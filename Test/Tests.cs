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
        public void ApsFixedLengthCombinationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
            var expected = new StringBuilder();
            expected.AppendLine("00");
            expected.AppendLine("10");
            expected.AppendLine("01");
            expected.AppendLine("11");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void ApsVariableLengthCombinationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddArbitraryPasswordSection("01", 2)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
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
        public void ApsNoSingleCharSequenceCombinationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(1)
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
            var expected = new StringBuilder();
            expected.AppendLine("10");
            expected.AppendLine("01");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void ApsFixedLengthNoSingleCharSequenceCombinationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(1)
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
            var expected = new StringBuilder();
            expected.AppendLine("10");
            expected.AppendLine("01");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void FpsFixedLengthUpperAndLowerCombinationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddFixedPasswordSection("ab", null, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
            var expected = new StringBuilder();
            expected.AppendLine("ab");
            expected.AppendLine("Ab");
            expected.AppendLine("aB");
            expected.AppendLine("AB");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void FpsVariableLengthUpperAndLowerCombinationsStringTest()
        {
            var passwordPattern = new PasswordPatternBuilder(2)
                .AddFixedPasswordSection("ab", 0, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
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

            var actual = passwordPattern.GetCombinationCount();
            var expected = 7ul;

            Assert.Equal(expected, actual);
        }
    }
}
