using Microsoft.Extensions.Logging;
using Moq;
using Simplicity.BusinessLogic;
using Simplicity.Database;
using Simplicity.Infrastructure;
using Simplicity.WebApi;

namespace SimplicityUnitTests
{
    /// <summary>
    /// Oh my lordy wordy! We are now drowning in dependencies for this class.
    /// </summary>
    public class RocketLaunchingLogicTests
    {
        [Fact]
        public async Task RocketLaunchingLogic_TriesToLaunchARocketAgain_IfItDoesNotInitiallyFindOne()
        {
            var thrustCalculatorMock = new Mock<IThrustCalculator>();
            var rocketDatabaseRetrieverMock = new Mock<IRocketDatabaseRetriever>();
            var rocketQueuePollerMock = new Mock<IRocketQueuePoller>();
            var rocketLaunchingServiceMock = new Mock<IRocketLaunchingService>();
            var loggerMock = new Mock<ILogger<RocketLaunchingLogic>>();
            var coordinateCalculatorMock = new Mock<ICoordinateCalculator>();
            var asyncDelayMock = new Mock<IAsyncDelay>();
            var rocketLaunchingLogic = new RocketLaunchingLogic(
                thrustCalculatorMock.Object,
                rocketDatabaseRetrieverMock.Object,
                rocketQueuePollerMock.Object,
                rocketLaunchingServiceMock.Object,
                loggerMock.Object,
                coordinateCalculatorMock.Object,
                asyncDelayMock.Object
            );

            // Wow! We now have 7 different dependencies that we need to orchestrate in order to test this class.
            // This is a code smell that we should fix before we try to write some tests here.
        }
    }
}