using Lib.Suppressors;
using Xunit;

namespace Test.Supressors
{
    public class CharOccurenceSuppressorTests
    {
        [Fact]
        public void NoDuplicatesTest()
        {
            var supressor = new CharOccurenceSuppressor();

            var actual = supressor.BreaksRestrictions("abcd");

            Assert.False(actual);
        }

        [Fact]
        public void SpecificDuplicatesNotBreaksTest()
        {
            var supressor = new CharOccurenceSuppressor(trackedChars: "ab");

            var actual = supressor.BreaksRestrictions("abccdd");

            Assert.False(actual);
        }

        [Fact]
        public void NoSpecificDuplicatesBreaksTest()
        {
            var supressor = new CharOccurenceSuppressor(trackedChars: "!ab");

            var actual = supressor.BreaksRestrictions("abccdd");

            Assert.True(actual);
        }

        [Fact]
        public void DifferentCaseDuplicatesTest()
        {
            var supressor = new CharOccurenceSuppressor();

            var actual = supressor.BreaksRestrictions("abBcd");

            Assert.True(actual);
        }

        [Fact]
        public void DifferentCaseDuplicatesCaseSensitiveTest()
        {
            var supressor = new CharOccurenceSuppressor(ignoreCase: false);

            var actual = supressor.BreaksRestrictions("abBcd");

            Assert.False(actual);
        }
    }
}
