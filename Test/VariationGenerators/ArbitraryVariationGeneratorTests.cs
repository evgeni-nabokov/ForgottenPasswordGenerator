using System.Collections.Generic;
using System.Text;
using Lib.Suppressors;
using Lib.VariationGenerators;
using Xunit;
using CharCase = Lib.VariationGenerators.CharCase;

namespace Test.VariationGenerators
{
    public class ArbitraryVariationGeneratorTests
    {
        [Fact]
        public void FixedLengthTest()
        {
            var generator = new ArbitraryVariationGenerator("01", 2, 2);

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("00");
            expected.AppendLine("10");
            expected.AppendLine("01");
            expected.AppendLine("11");

            Assert.Equal(expected.ToString(), actual);
            Assert.Equal<ulong>(4, generator.VariationNumber);
            Assert.Equal<ulong>(4, generator.LoopNumber);
            Assert.Equal<ulong>(4, generator.LoopCount);
        }

        [Fact]
        public void VariableLengthTest()
        {
            var generator = new ArbitraryVariationGenerator("01", 0, 2);

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("");
            expected.AppendLine("0");
            expected.AppendLine("1");
            expected.AppendLine("00");
            expected.AppendLine("10");
            expected.AppendLine("01");
            expected.AppendLine("11");

            Assert.Equal(expected.ToString(), actual);
            Assert.Equal<ulong>(7, generator.VariationNumber);
            Assert.Equal<ulong>(7, generator.LoopNumber);
            Assert.Equal<ulong>(7, generator.LoopCount);
        }

        [Fact]
        public void FixedLengthNoDuplicatesTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor();
            var generator = new ArbitraryVariationGenerator("01", 2, 2,
                CharCase.AsDefined, new List<ISuppressor> { supressor });

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("10");
            expected.AppendLine("01");

            Assert.Equal(expected.ToString(), actual);
            Assert.Equal<ulong>(2, generator.VariationNumber);
            Assert.Equal<ulong>(4, generator.LoopNumber);
            Assert.Equal<ulong>(4, generator.LoopCount);
        }

        [Fact]
        public void VariableLengthNoDuplicatesTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor();
            var generator = new ArbitraryVariationGenerator("01", 0, 2,
                CharCase.AsDefined, new List<ISuppressor> { supressor });

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("");
            expected.AppendLine("0");
            expected.AppendLine("1");
            expected.AppendLine("10");
            expected.AppendLine("01");

            Assert.Equal(expected.ToString(), actual);
            Assert.Equal<ulong>(5, generator.VariationNumber);
            Assert.Equal<ulong>(7, generator.LoopNumber);
            Assert.Equal<ulong>(7, generator.LoopCount);
        }
    }
}
