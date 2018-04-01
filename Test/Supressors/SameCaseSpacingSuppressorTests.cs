using Lib.Suppressors;
using Xunit;

namespace Test.Supressors
{
    public class SameCaseSpacingSuppressorTests
    {
        [Fact]
        public void NoSameCaseTest()
        {
            var supressor = new SameCaseSpacingSuppressor(1, 1);

            var actual = supressor.BreaksRestrictions("abcd");

            Assert.False(actual);
        }

        [Fact]
        public void SingleSameCaseTest()
        {
            var supressor = new SameCaseSpacingSuppressor(1, 1);

            var actual = supressor.BreaksRestrictions("Abcd");

            Assert.False(actual);
        }

        [Fact]
        public void TwoSameCaseNotBreaksTest()
        {
            var supressor = new SameCaseSpacingSuppressor(1, 1);

            var actual = supressor.BreaksRestrictions("AbCd");

            Assert.False(actual);
        }

        [Fact]
        public void TwoSameCaseBreaksTest()
        {
            var supressor = new SameCaseSpacingSuppressor(1, 1);

            var actual = supressor.BreaksRestrictions("AbcD");

            Assert.True(actual);
        }
    }
}
