using FixingStaticCode.BusinessLogic;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Moq;

namespace FixingStaticCodeUnitTests
{
    /// <summary>
    /// We can now test the thrust calculator with determinism, because we are using an abstracted time provider via ISystemClock
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
        // Case 3 - used to understand what would happen if we had very big numbers to calculate thrust with
        //[InlineData(2147483647, 2147483647, 2147483647, 2147483647, 2147483647, 2147483647, "2023-01-01T01:59:59Z", 2147483647, "Testing max values before 2am UTC")]
        public void ThrustCalculator_ReturnsExpectedValues(
            int thrustValue1, int thrustValue2, int thrustValue3, int thrustValue4, int thrustValue5, int numberOfSloths,
            string timeString,
            int expectedThrustValue,
            string explanation
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

            result.Should().Be(expectedThrustValue, explanation);
        }

        [Theory]
        // Case 1 - guarantee overflow
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, "2023-01-01T01:59:59Z", "Arithmetic overflow with max values before 2am UTC")]
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, "2023-01-01T02:00:00Z", "Arithmetic overflow with max values at 2am UTC")]
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, "2023-01-01T03:00:00Z", "Arithmetic overflow with max values after 2am UTC")]
        // Case 2 - a different variation of overflow
        [InlineData(999999999, 500000000, 1, 10, 3, 999999999, "2023-01-01T01:59:59Z", "Arithmetic overflow with max values before 2am UTC")]
        [InlineData(999999999, 500000000, 1, 10, 3, 999999999, "2023-01-01T02:00:00Z", "Arithmetic overflow with max values at 2am UTC")]
        [InlineData(999999999, 500000000, 1, 10, 3, 999999999, "2023-01-01T03:00:00Z", "Arithmetic overflow with max values after 2am UTC")]
        // Case 3 - testing negative overflow
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, -1 * int.MaxValue, "2023-01-01T01:59:59Z", "Arithmetic overflow with max values before 2am UTC")]
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, -1 * int.MaxValue, "2023-01-01T02:00:00Z", "Arithmetic overflow with max values at 2am UTC")]
        [InlineData(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, -1 * int.MaxValue, "2023-01-01T03:00:00Z", "Arithmetic overflow with max values after 2am UTC")]
        public void CalculateThrust_ThrowsOverflowException_WhenArithmeticOverflowHappens(
            int thrustValue1, int thrustValue2, int thrustValue3, int thrustValue4, int thrustValue5, int numberOfSloths,
            string timeString,
            string explanation
            )
        {
            var mockClock = new Mock<ISystemClock>();

            // Parse the time (which will make it a local time, and then convert back to UTC)
            var parsedTime = DateTime.Parse(timeString).ToUniversalTime();
            // Return a fake output from our mocked ISystemClock to bring determinism back into our test
            mockClock.Setup(method => method.UtcNow).Returns(parsedTime);

            // Provide the mock clock to the class
            var thrustCalculator = new ThrustCalculator(mockClock.Object);

            Func<int> act = () => thrustCalculator.CalculateThrust(
                thrustValue1,
                thrustValue2,
                thrustValue3,
                thrustValue4,
                thrustValue5,
                numberOfSloths
            );

            // Fluent assertions catches the exception which bubbles out of the RocketLauncher
            act.Should().Throw<OverflowException>(explanation);
        }
    }
}
