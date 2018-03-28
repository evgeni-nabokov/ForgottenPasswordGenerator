using System.Text;
using Lib.PasswordPattern;
using Lib.PasswordPattern.Extensions;
using Xunit;

namespace Test
{
    public class StringListPasswordSectionTests
    {
        [Fact]
        public void StringTest()
        {
            var passwordPattern = new PasswordPatternBuilder()
                .AddStringListPasswordSection(new[] { "cat", "dog", "mouse" })
                .Build();

            var actual = passwordPattern.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("cat");
            expected.AppendLine("dog");
            expected.AppendLine("mouse");
            Assert.Equal(expected.ToString(), actual);
        }
    }
}
