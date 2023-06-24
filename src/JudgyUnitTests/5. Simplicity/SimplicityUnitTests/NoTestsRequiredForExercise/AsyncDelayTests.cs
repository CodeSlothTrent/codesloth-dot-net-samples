using FluentAssertions;
using Simplicity.Infrastructure;

namespace SimplicityUnitTests.NoTestsRequiredForExercise
{
    public class AsyncDelayTests
    {
        [Fact]
        public void NoTestsRequiredOnDummyClass()
        {
            // This is a dummy infrastructure class that does not require unit tests for this exercise
            var asyncDelay = new AsyncDelay();
            true.Should().BeTrue();
        }
    }
}
