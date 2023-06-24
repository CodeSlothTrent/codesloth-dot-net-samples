using FluentAssertions;
using Simplicity.WebApi;

namespace SimplicityUnitTests.NoTestsRequiredForExercise
{
    public class RocketLaunchingApiTests
    {
        [Fact]
        public void NoTestsRequiredOnDummyClass()
        {
            // This is a dummy infrastructure class that does not require unit tests for this exercise
            var asyncDelay = new RocketLaunchingApi(new HttpClient());
            true.Should().BeTrue();
        }
    }
}
