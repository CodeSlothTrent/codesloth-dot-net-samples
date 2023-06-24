using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Moq;
using Simplicity.BusinessLogic.Navigation;
using Simplicity.Database;

namespace SimplicityUnitTests
{
    /// <summary>
    /// We can now test the thrust calculator with determinism, because we are using an abstracted time provider via ISystemClock
    /// </summary>
    public class RocketNavigationTests
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
        public async Task CalculateThrust_CaclulatesValidThrust_AtAppropriateTimesOfDay(
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

            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();
            databaseRetrieverMock.Setup(method => method.FindRocketStatistics(It.IsAny<int>()))
                .ReturnsAsync(new RocketThrustStatistics(
                    thrustValue1,
                    thrustValue2,
                    thrustValue3,
                    thrustValue4,
                    thrustValue5
            ));

            // Provide the mock clock to the class
            var rocketNavigation = new RocketNavigation(mockClock.Object, databaseRetrieverMock.Object);

            var result = await rocketNavigation.CalculateThrust(1, numberOfSloths);

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
        public async Task CalculateThrust_ThrowsOverflowException_WhenArithmeticOverflowHappens(
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

            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();
            databaseRetrieverMock.Setup(method => method.FindRocketStatistics(It.IsAny<int>()))
                .ReturnsAsync(new RocketThrustStatistics(
                    thrustValue1,
                    thrustValue2,
                    thrustValue3,
                    thrustValue4,
                    thrustValue5
            ));

            var rocketNavigation = new RocketNavigation(mockClock.Object, databaseRetrieverMock.Object);

            Func<Task<int>> act = () => rocketNavigation.CalculateThrust(1, numberOfSloths);

            // Fluent assertions catches the exception which bubbles out of the RocketLauncher
            await act.Should().ThrowAsync<OverflowException>(explanation);
        }

        [Fact]
        public void CalculateCoordinatesToLand_ThrowsException_WhenGivenNoFood()
        {
            var mockClock = new Mock<ISystemClock>();
            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();

            var rocketNavigation = new RocketNavigation(mockClock.Object, databaseRetrieverMock.Object);
            Func<(int lat, int lon)> act = () => rocketNavigation.CalculateCoordinatesToLand(1, (FoodForJourney)null);

            act.Should().Throw<ArgumentException>("Food should be expected in calculating coordinates to land");
        }

        // Establish a testing case that our coordinates are calculated based off the first meal. This will allow us to simplify 
        // our subsequent testing that covers the current business rules
        [Theory]
        [InlineData(new[] { PretendDatabaseClient.FoodSushi, PretendDatabaseClient.FoodSlightlyToxicLeaves }, 456, 789)]
        [InlineData(new[] { PretendDatabaseClient.FoodSlightlyToxicLeaves, PretendDatabaseClient.FoodSushi }, 123, 456)]
        public void CalculateCoordinatesToLand_CalculatesCoordinates_BasedOffFirstMeal(string[] foods, int lat, int lon)
        {
            var mockClock = new Mock<ISystemClock>();
            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();

            var rocketNavigation = new RocketNavigation(mockClock.Object, databaseRetrieverMock.Object);
            var foodForJourney = new FoodForJourney
            {
                Foods = foods,
                NumberOfCourses = 2
            };
            var coordinates = rocketNavigation.CalculateCoordinatesToLand(1, foodForJourney);
            coordinates.Should().BeEquivalentTo((lat, lon));
        }

        [Theory]
        [InlineData(1, PretendDatabaseClient.FoodSushi, 456, 789)]
        [InlineData(10, PretendDatabaseClient.FoodSushi, 111, 000)]
        [InlineData(11, PretendDatabaseClient.FoodSushi, 111, 222)]
        [InlineData(1, PretendDatabaseClient.FoodSlightlyToxicLeaves, 123, 456)]
        [InlineData(10, PretendDatabaseClient.FoodSlightlyToxicLeaves, 111, 000)]
        [InlineData(11, PretendDatabaseClient.FoodSlightlyToxicLeaves, 444, 444)]
        [InlineData(1, PretendDatabaseClient.FoodPasta, 000, 111)]
        [InlineData(9, PretendDatabaseClient.FoodPasta, 000, 111)]
        [InlineData(10, PretendDatabaseClient.FoodPasta, 111, 000)]
        public void CalculateCoordinatesToLand_CalculatesCoordinates_BasedOffSlothCountAndMeal(int numberOfSloths, string food, int lat, int lon)
        {
            var mockClock = new Mock<ISystemClock>();
            var databaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();

            var rocketNavigation = new RocketNavigation(mockClock.Object, databaseRetrieverMock.Object);
            var foodForJourney = new FoodForJourney
            {
                Foods = new[] { food },
                NumberOfCourses = 2
            };
            var coordinates = rocketNavigation.CalculateCoordinatesToLand(numberOfSloths, foodForJourney);
            coordinates.Should().BeEquivalentTo((lat, lon));
        }
    }
}
