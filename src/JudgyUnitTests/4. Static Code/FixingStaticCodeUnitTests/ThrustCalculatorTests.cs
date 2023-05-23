using FixingStaticCode.BusinessLogic;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Moq;

namespace FixingStaticCodeUnitTests
{
    /// <summary>
    /// While the thrust calculator itself has no dependencies, we can't test its functionality with determinism
    /// </summary>
    public class ThrustCalculatorTests
    {
        [Theory]
        // Case 1
        [InlineData(1, 1, 1, 1, 1, 1, "2023-01-01T01:59:59Z", 5, "Hour before 2am UTC has no modifier")]
        [InlineData(1, 1, 1, 1, 1, 1, "2023-01-01T02:00:00Z", 5, "Hour at 2am UTC has no modifier")]
        [InlineData(1, 1, 1, 1, 1, 1, "2023-01-01T03:00:00Z", 15, "Hour after 2am UTC has +10 modifier")]
        // Case 2 - ensuring that we don't always return the above values
        [InlineData(1, 2, 5, 3, 9, 7, "2023-01-01T01:59:59Z", 140, "Hour before 2am UTC has no modifier")]
        [InlineData(1, 2, 5, 3, 9, 7, "2023-01-01T02:00:00Z", 140, "Hour at 2am UTC has no modifier")]
        [InlineData(1, 2, 5, 3, 9, 7, "2023-01-01T03:00:00Z", 150, "Hour after 2am UTC has +10 modifier")]
        public void ThrustCalculator_ReturnsExpectedValues(
            int thrustValue1, int thrustValue2, int thrustValue3, int thrustValue4, int thrustValue5, int numberOfSloths,
            string timeString,
            int expectedThrustValue,
            string exaplanation
        )
        {
            var mockClock = new Mock<ISystemClock>();

            // Parse the time (which will make it a local time, and then convert back to UTC)
            var parsedTime = DateTime.Parse(timeString).ToUniversalTime();
            // Return a fake output from our mocked ISystemClock to bring determinism back into our test
            mockClock.Setup(method => method.UtcNow).Returns(parsedTime);

            // Provide the mock clock to the class
            var thrustCalculator = new ThrustCalculator(mockClock.Object);

            var result = thrustCalculator.CalculateThrust(
                thrustValue1,
                thrustValue2,
                thrustValue3,
                thrustValue4,
                thrustValue5,
                numberOfSloths
            );

            result.Should().Be(expectedThrustValue, exaplanation);
        }
    }
}
