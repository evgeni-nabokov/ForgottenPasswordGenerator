using System.Collections.Generic;
using System.Text;
using Lib.Suppressors;
using Lib.VariationGenerators;
using Xunit;
using CharCase = Lib.VariationGenerators.CharCase;

namespace Test
{
    public class NumberRangeVariationGeneratorTests
    {
        [Fact]
        public void SimpleStringTest()
        {
            var generator = new NumberRangeVariationGenerator(-2, 2);

            var actual = generator.GetVariationsString();

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
