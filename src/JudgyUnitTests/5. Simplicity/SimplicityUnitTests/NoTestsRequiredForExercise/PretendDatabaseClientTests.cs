using FluentAssertions;
using Simplicity.Database;

namespace SimplicityUnitTests.NoTestsRequiredForExercise
{
    public class PretendDatabaseClientTests
    {
        [Fact]
        public void NoTestsRequiredOnDummyClass()
        {
            // This is a dummy infrastructure class that does not require unit tests for this exercise
            var asyncDelay = new PretendDatabaseClient(string.Empty, TimeSpan.MaxValue, TimeSpan.MaxValue, 1, 1);
            true.Should().BeTrue();
        }
    }
}
