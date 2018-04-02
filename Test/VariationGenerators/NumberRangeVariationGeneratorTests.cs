using System.Text;
using Lib.VariationGenerators;
using Xunit;

namespace Test.VariationGenerators
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
            Assert.Equal<ulong>(5, generator.VariationNumber);
            Assert.Equal<ulong>(5, generator.LoopNumber);
            Assert.Equal<ulong>(5, generator.LoopCount);
        }
    }
}
