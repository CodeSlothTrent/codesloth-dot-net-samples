using FixingConcreteDependencies.BusinessLogic;
using FluentAssertions;

namespace FixingConcreteDependenciesUnitTests
{
    /// <summary>
    /// While the thrust calculator itself has no dependencies, we can't test its functionality with determinism
    /// </summary>
    public class ThrustCalculatorTests
    {
        [Theory]
        [InlineData(1,1,1,1,1,1, 5, "thrust is the sum of all values multiplied by 1 as this is the number of sloths to launch")]
        public void ThrustCalculator_ReturnsExpectedValues(int thrustValue1, int thrustValue2, int thrustValue3, int thrustValue4, int thrustValue5, int numberOfSloths, int expectedThrustValue, string exaplanation)
        {
            var thrustCalculator = new ThrustCalculator();
            var result = thrustCalculator.CalculateThrust(
                thrustValue1,
                thrustValue2,
                thrustValue3,
                thrustValue4,
                thrustValue5,
                numberOfSloths
            );
            
            // Oh no! This test sometimes fails. But why???? Perhaps it is to do with that pesky DateTime.Now?
            result.Should().Be(expectedThrustValue);
        }
    }
}
