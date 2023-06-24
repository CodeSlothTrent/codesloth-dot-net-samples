using FluentAssertions;
using Simplicity.Database;

namespace SimplicityUnitTests
{
    public class RocketDatabaseRetrieverTests
    {
        [Fact]
        public void NoTestsRequiredOnDummyClass()
        {
            // This is a dummy infrastructure class that does not require unit tests for this exercise
            var poller = new RocketDatabaseRetriever(null, null);
            true.Should().BeTrue();
        }
    }
}
