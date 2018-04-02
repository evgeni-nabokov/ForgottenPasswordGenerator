using Lib.Suppressors;
using Xunit;

namespace Test.Supressors
{
    public class AdjacentSameCaseSuppressorTests
    {
        [Fact]
        public void NoSameCaseTestNotBreaks()
        {
            var supressor = new AdjacentSameCaseSuppressor();

            var actual = supressor.BreaksRestrictions("abbc");

            Assert.False(actual);
        }

        [Fact]
        public void SingleSameCaseNotBreaksTest()
        {
            var supressor = new AdjacentSameCaseSuppressor();

            var actual = supressor.BreaksRestrictions("Abbc");

            Assert.False(actual);
        }

        [Fact]
        public void AdjacentSameCaseBreaksTest()
        {
            var supressor = new AdjacentSameCaseSuppressor();

            var actual = supressor.BreaksRestrictions("ABc");

            Assert.True(actual);
        }

        [Fact]
        public void NotAdjacentSameCaseNotBreaksTest()
        {
            var supressor = new AdjacentSameCaseSuppressor();

            var actual = supressor.BreaksRestrictions("AbC");

            Assert.False(actual);
        }
    }
}
