using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Simplicity.Database;
using Simplicity.Database.DTO;

namespace SimplicityUnitTests
{
    public class RocketDatabaseRetrieverTests
    {
        [Fact]
        public async Task GetFoodToFeedSlothsOnTheirJourney_ReturnsNull_WhenDatabaseLookupFails()
        {
            var pretendDatabaseClientMock = new Mock<IPretendDatabaseClient>();
            pretendDatabaseClientMock.Setup(method => method.FindFood(It.IsAny<string>())).ThrowsAsync(new Exception());
            var loggerMock = new Mock<ILogger<RocketDatabaseRetriever>>();
            var rocketDatabaseRetriever = new RocketDatabaseRetriever(pretendDatabaseClientMock.Object, loggerMock.Object);

            var result = await rocketDatabaseRetriever.GetFoodToFeedSlothsOnTheirJourney(1, 1);
            result.Should().BeNull();
        }

        [Fact]
        public async Task FindRocketStatistics_PropagatesException_WhenDatabaseLookupFails()
        {
            var pretendDatabaseClientMock = new Mock<IPretendDatabaseClient>();
            pretendDatabaseClientMock.Setup(method => method.FindRocketStatistics(It.IsAny<string>())).ThrowsAsync(new Exception());
            var loggerMock = new Mock<ILogger<RocketDatabaseRetriever>>();
            var rocketDatabaseRetriever = new RocketDatabaseRetriever(pretendDatabaseClientMock.Object, loggerMock.Object);

            Func<Task<RocketThrustStatistics>> act = () => rocketDatabaseRetriever.FindRocketStatistics(1);
            await act.Should().ThrowAsync<Exception>();
        }
    }
}
