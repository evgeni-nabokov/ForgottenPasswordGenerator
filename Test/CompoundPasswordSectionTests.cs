using System.Text;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Xunit;

namespace Test
{
    public class CompoundPasswordSectionTests
    {
        [Fact]
        public void SimplePatternStringTest()
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
        public void SimplePatternStringTestWithDuplicates()
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
