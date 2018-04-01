using System.Collections.Generic;
using System.Text;
using Lib.VariationGenerators;
using Xunit;

namespace Test
{
    public class StringListVariationGeneratorTests
    {
        [Fact]
        public void SimpleTest()
        {
            var generator = new StringListVariationGenerator(new List<string> { "cat", "dog", "dog", "mouse" });

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("cat");
            expected.AppendLine("dog");
            expected.AppendLine("mouse");

            Assert.Equal(expected.ToString(), actual);
        }
    }
}
