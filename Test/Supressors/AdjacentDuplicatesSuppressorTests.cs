using Lib.Suppressors;
using Xunit;

namespace Test.Supressors
{
    public class AdjacentDuplicatesSuppressorTests
    {
        [Fact]
        public void DuplicatesCaseInsensitiveTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor();

            var actual = supressor.BreaksRestrictions("abbc");

            Assert.True(actual);
        }

        [Fact]
        public void NoDuplicatesTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor();

            var actual = supressor.BreaksRestrictions("abc");

            Assert.False(actual);
        }

        [Fact]
        public void DifferentCaseCaseInsensitiveTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor();

            var actual = supressor.BreaksRestrictions("abBc");

            Assert.True(actual);
        }

        [Fact]
        public void DifferentCaseCaseSensitiveTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor(1, 1, null, false);

            var actual = supressor.BreaksRestrictions("abBc");

            Assert.False(actual);
        }

        [Fact]
        public void MaxLengthDifferentCaseCaseInsensitiveTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor(1, 2);

            var actual = supressor.BreaksRestrictions("abBbc");

            Assert.True(actual);
        }

        [Fact]
        public void MaxLengthDifferentCaseCaseInsensitiveNotBreaksTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor(1, 2);

            var actual = supressor.BreaksRestrictions("abBc");

            Assert.False(actual);
        }

        [Fact]
        public void TrackingCharDifferentCaseCaseInsensitiveTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor(1, 1, "bC");

            var actual = supressor.BreaksRestrictions("aabBc");

            Assert.True(actual);
        }

        [Fact]
        public void TrackingCharDifferentCaseCaseSensitiveNotBreaksTest()
        {
            var supressor = new AdjacentDuplicatesSuppressor(1, 1, "ac", false);

            var actual = supressor.BreaksRestrictions("abBc");

            Assert.False(actual);
        }

    }
}
