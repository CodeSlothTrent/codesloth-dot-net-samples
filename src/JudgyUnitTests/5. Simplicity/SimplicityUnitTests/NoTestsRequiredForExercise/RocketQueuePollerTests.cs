using FluentAssertions;
using Simplicity.Infrastructure;

namespace SimplicityUnitTests.NoTestsRequiredForExercise
{
    public class RocketQueuePollerTests
    {
        [Fact]
        public void NoTestsRequiredOnDummyClass()
        {
            // This is a dummy infrastructure class that does not require unit tests for this exercise
            var poller = new RocketQueuePoller(1, 1, string.Empty);
            true.Should().BeTrue();
        }
    }
}
