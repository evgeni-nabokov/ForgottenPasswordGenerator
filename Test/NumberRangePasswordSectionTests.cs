using System.Text;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Xunit;

namespace Test
{
    public class NumberRangePasswordSectionTests
    {
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
    }
}
