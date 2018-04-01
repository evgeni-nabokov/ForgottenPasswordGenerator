using System.Collections.Generic;
using System.Text;
using Lib.CharMappers;
using Lib.Suppressors;
using Lib.VariationGenerators;
using Xunit;
using CharCase = Lib.VariationGenerators.CharCase;

namespace Test
{
    public class FixedVariationGeneratorTests
    {
        [Fact]
        public void FixedLengthUpperAndLowerVariationsStringTest()
        {
            var generator = new FixedVariationGenerator("ab", 2, CharCase.UpperAndLower);

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("ab");
            expected.AppendLine("Ab");
            expected.AppendLine("aB");
            expected.AppendLine("AB");

            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void VariableLengthUpperAndLowerVariationsStringTest()
        {
            var generator = new FixedVariationGenerator("ab", 0, CharCase.UpperAndLower);

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("");
            expected.AppendLine("a");
            expected.AppendLine("A");
            expected.AppendLine("ab");
            expected.AppendLine("Ab");
            expected.AppendLine("aB");
            expected.AppendLine("AB");

            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void VariableLengthVariationLoopCountTest()
        {
            var generator = new FixedVariationGenerator("ab", 0, CharCase.UpperAndLower);

            var actual = generator.LoopCount;
            const ulong expected = 7ul;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void VariableLengthVariationNumberTest()
        {
            var generator = new FixedVariationGenerator("ab", 0, CharCase.UpperAndLower);

            const ulong expected = 7ul;
            var variants = generator.GetVariationsString();

            Assert.Equal(expected, generator.VariationNumber);
            Assert.Equal(expected, generator.LoopNumber);
        }

        [Fact]
        public void SameCaseDuplicatesTest()
        {
            var supressor = new AdjacentSameCaseSuppressor();
            var generator = new FixedVariationGenerator("abc", 3,
                CharCase.UpperAndLower, new List<ISuppressor> { supressor });

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("abc");
            expected.AppendLine("Abc");
            expected.AppendLine("aBc");
            expected.AppendLine("abC");
            expected.AppendLine("AbC");

            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void SameCaseSpacingTest()
        {
            var supressor = new SameCaseSpacingSuppressor(1, 1);
            var generator = new FixedVariationGenerator("abcd", null,
                CharCase.UpperAndLower, new List<ISuppressor> { supressor });

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("abcd");
            expected.AppendLine("Abcd");
            expected.AppendLine("aBcd");
            expected.AppendLine("abCd");
            expected.AppendLine("AbCd");
            expected.AppendLine("abcD");
            expected.AppendLine("aBcD");
            Assert.Equal(expected.ToString(), actual);
        }

        [Fact]
        public void RussianCharMapperStringTest()
        {
            var generator = new FixedVariationGenerator("ÈˆÛÍÂÌ„¯˘Áı˙Ù˚‚‡ÔÓÎ‰Ê˝ˇ˜ÒÏËÚ¸·˛∏…÷” ≈Õ√ÿŸ«’⁄‘€¬¿œ–ŒÀƒ∆›ﬂ◊—Ã»“‹¡ﬁ®",
                null, CharCase.AsDefined, null, new RussianToEnglishMapper());

            var actual = generator.GetVariationsString();
            var expected = new StringBuilder();
            expected.AppendLine("qwertyuiop[]asdfghjkl;'zxcvbnm,.`QWERTYUIOP{}ASDFGHJKL:\"ZXCVBNM<>~");

            Assert.Equal(expected.ToString(), actual);
        }
    }
}
