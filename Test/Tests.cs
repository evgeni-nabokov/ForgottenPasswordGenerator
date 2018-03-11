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
        public void Test1()
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
        public void Test2()
        {
            var passwordPattern = new PasswordPatternBuilder(1)
                .AddArbitraryPasswordSection("01", 2, 1)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
            var expected = new StringBuilder();
            expected.AppendLine("0");
            expected.AppendLine("1");
            expected.AppendLine("00");
            expected.AppendLine("10");
            expected.AppendLine("01");
            expected.AppendLine("11");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Test3()
        {
            var passwordPattern = new PasswordPatternBuilder(1, 1)
                .AddArbitraryPasswordSection("01", 2, 2)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
            var expected = new StringBuilder();
            expected.AppendLine("10");
            expected.AppendLine("01");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Test4()
        {
            var passwordPattern = new PasswordPatternBuilder(1, 2)
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
        public void Test5()
        {
            var passwordPattern = new PasswordPatternBuilder(1, 2)
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
        public void Test6()
        {
            var passwordPattern = new PasswordPatternBuilder(1, 2)
                .AddFixedPasswordSection("ab", 1, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetCombinationsString();
            var expected = new StringBuilder();
            expected.AppendLine("a");
            expected.AppendLine("A");
            expected.AppendLine("ab");
            expected.AppendLine("Ab");
            expected.AppendLine("aB");
            expected.AppendLine("AB");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void Test7()
        {
            var passwordPattern = new PasswordPatternBuilder(1, 2)
                .AddFixedPasswordSection("ab", 1, CharCase.UpperAndLower)
                .Build();

            var actual = passwordPattern.GetCombinationCount();
            var expected = 6ul;

            Assert.Equal(expected, actual);
        }
    }
}
