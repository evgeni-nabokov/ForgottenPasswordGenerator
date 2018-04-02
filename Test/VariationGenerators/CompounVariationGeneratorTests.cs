using System.Collections.Generic;
using System.Text;
using Lib.VariationGenerators;
using Xunit;

namespace Test.VariationGenerators
{
    public class CompounVariationGeneratorTests
    {
        [Fact]
        public void SimpleVariationNumberTest()
        {
            var generator1 = new FixedVariationGenerator("ab", 2, CharCase.UpperAndLower);
            var generator2 = new NumberRangeVariationGenerator(0, 1);
            var compoundGenerator = new CompoundVariationGenerator(new List<IVariationGenerator> {
                generator1,
                generator2
            });

            var actual = compoundGenerator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("ab0");
            expected.AppendLine("Ab0");
            expected.AppendLine("aB0");
            expected.AppendLine("AB0");
            expected.AppendLine("ab1");
            expected.AppendLine("Ab1");
            expected.AppendLine("aB1");
            expected.AppendLine("AB1");

            Assert.Equal(expected.ToString(), actual);
            Assert.Equal<ulong>(8, compoundGenerator.VariationNumber);
            Assert.Equal<ulong>(8, compoundGenerator.LoopNumber);
            Assert.Equal<ulong>(8, compoundGenerator.LoopCount);
        }
    }
}
