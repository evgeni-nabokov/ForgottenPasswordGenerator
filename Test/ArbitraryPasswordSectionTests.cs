using System.Text;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Xunit;

namespace Test
{
    public class ArbitraryPasswordSectionTests
    {
        [Fact]
        public void FixedLengthVariationsStringTest()
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
        public void VariableLengthVariationsStringTest()
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
        public void NoSingleCharSequenceVariationsStringTest()
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
        public void FixedLengthNoSingleCharSequenceVariationsStringTest()
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
    }
}
