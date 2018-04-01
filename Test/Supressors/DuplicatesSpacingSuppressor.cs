using Lib.Suppressors;
using Xunit;

namespace Test.Supressors
{
    public class DuplicatesSpacingSuppressorTests
    {
        [Fact]
        public void NoDuplicatesTest()
        {
            var supressor = new DuplicatesSpacingSuppressor();

            var actual = supressor.BreaksRestrictions("abcd");

            Assert.False(actual);
        }

        [Fact]
        public void OneDuplicateNotBreaksTest()
        {
            var supressor = new DuplicatesSpacingSuppressor();

            var actual = supressor.BreaksRestrictions("abad");

            Assert.False(actual);
        }

        [Fact]
        public void OneDuplicateBreaksMinSpaceTest()
        {
            var supressor = new DuplicatesSpacingSuppressor();

            var actual = supressor.BreaksRestrictions("aacd");

            Assert.True(actual);
        }

        [Fact]
        public void OneDuplicateBreaksMaxSpaceTest()
        {
            var supressor = new DuplicatesSpacingSuppressor();

            var actual = supressor.BreaksRestrictions("abca");

            Assert.True(actual);
        }
    }
}
